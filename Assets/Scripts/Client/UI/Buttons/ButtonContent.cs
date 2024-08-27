using Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class ButtonContent : UIBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TooltipReference tooltips;
        [SerializeField] private BalanceReference balance;
        [SerializeField] private RenderingReference rendering;
        [SerializeField] private InputReference input;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Image contentImage;
        [SerializeField] private Image cooldownImage;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private TextMeshProUGUI chargeText;
        [SerializeField] private Button button;
        [SerializeField] private TooltipAlignment tooltipAlignment = TooltipAlignment.FromTop;

        public ButtonSlot ButtonSlot { get; private set; }
        public ButtonContentType ContentType => data.ActionType;
        public Image ContentImage => contentImage;

        private readonly ActionButtonData data = new(0, ButtonContentType.Empty);
        private PointerEventData manualPointerData;
        private SpellInfo spellInfo;

        private bool isPointerDown;
        private bool isHotkeyDown;

        private readonly char[] timerText = new char[3];
        private readonly char[] chargeCountText = new char[11];
        private bool showingTimer;

        public bool IsAlreadyPressed => isPointerDown || isHotkeyDown;

        public void Initialize(ButtonSlot buttonSlot)
        {
            manualPointerData = new PointerEventData(EventSystem.current);
            ButtonSlot = buttonSlot;

            UpdateContent();
        }

        public void Deinitialize()
        {
            ButtonSlot = null;
            isHotkeyDown = false;
            isPointerDown = false;
        }

        public void DoUpdate()
        {
            switch (data.ActionType)
            {
                case ButtonContentType.Spell:
                    UpdateSpell();
                    break;
                case ButtonContentType.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data.ActionType), $"Unknown button content: {data.ActionType} with id: {data.ActionId}");
            }
        }

        public void UpdateContent(ActionButtonData newData)
        {
            data.Modify(newData);

            UpdateContent();
        }

        public void Activate()
        {
            if (!enabled)
            {
                return;
            }

            switch (data.ActionType)
            {
                case ButtonContentType.Spell when balance.SpellInfosById.ContainsKey(data.ActionId):
                    input.CastSpell(data.ActionId);
                    break;
                case ButtonContentType.Empty:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data.ActionType), $"Unknown button content type: {data.ActionType}");
            }
        }

        public void Remove()
        {
            data.Reset();

            spellInfo = null;
            ContentImage.sprite = null;
            ContentImage.enabled = false;
            cooldownText.text = string.Empty;
            cooldownImage.fillAmount = 0;
            enabled = false;
        }

        public void HandleHotkeyState(HotkeyState state)
        {
            isHotkeyDown = state == HotkeyState.Pressed;
            if (isHotkeyDown && !isPointerDown)
            {
                button.OnPointerDown(manualPointerData);
            }
            else if (!isHotkeyDown && !isPointerDown)
            {
                button.OnPointerUp(manualPointerData);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltips.Show(spellInfo, rectTransform, tooltipAlignment, TooltipSize.Normal);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltips.Hide();
        }

        public void OnPointerDown(PointerEventData data)
        {
            isPointerDown = true;
        }

        public void OnPointerUp(PointerEventData data)
        {
            isPointerDown = false;
            if (isHotkeyDown)
            {
                button.OnPointerDown(manualPointerData);
            }
        }

        public void OnDrag(PointerEventData data)
        {
        }

        public void FromDrag(ButtonContent draggedContent)
        {
        }

        public void FromDrop(ButtonContent droppedContent)
        {
        }

        public void Replace(ButtonContent newContent)
        {
        }

        private void UpdateContent()
        {
            switch (data.ActionType)
            {
                case ButtonContentType.Spell when balance.SpellInfosById.ContainsKey(data.ActionId):
                    spellInfo = balance.SpellInfosById[data.ActionId];
                    ContentImage.sprite = rendering.SpellVisuals.ContainsKey(data.ActionId)
                        ? rendering.SpellVisuals[data.ActionId].SpellIcon
                        : rendering.DefaultSpellIcon;

                    ContentImage.enabled = true;
                    enabled = true;
                    break;
                case ButtonContentType.Spell:
                case ButtonContentType.Empty:
                    Remove();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data.ActionType), $"Unknown button content: {data.ActionType} with id: {data.ActionId}");
            }
        }

        private void UpdateSpell()
        {
            Player player = input.Player;
            if (player == null)
            {
                return;
            }

            int cooldownTimeLeft;
            int cooldownTime;
            var availableCharges = 0;
            var showTimer = showingTimer;

            if (!player.SpellHistory.HasGlobalCooldown || spellInfo.HasAttribute(SpellExtraAttributes.IgnoreGcd))
            {
                cooldownTimeLeft = cooldownTime = 0;
            }
            else
            {
                cooldownTimeLeft = player.SpellHistory.GlobalCooldownLeft;
                cooldownTime = player.SpellHistory.GlobalCooldown;
            }

            if (spellInfo.IsUsingCharges)
            {
                var hasCharge = player.SpellHistory.HasCharge(spellInfo, out SpellChargeCooldown chargeCooldown, out availableCharges);
                if (!hasCharge && chargeCooldown.ChargeTimeLeft > cooldownTimeLeft)
                {
                    showTimer = true;
                    cooldownTimeLeft = chargeCooldown.ChargeTimeLeft;
                    cooldownTime = chargeCooldown.ChargeTime;
                }
                else if (hasCharge && chargeCooldown != null)
                {
                    showTimer = false;
                    cooldownTimeLeft = chargeCooldown.ChargeTimeLeft;
                    cooldownTime = chargeCooldown.ChargeTime;
                }
            }
            else if (player.SpellHistory.HasCooldown(spellInfo.Id, out SpellCooldown spellCooldown) && spellCooldown.CooldownLeft > cooldownTimeLeft)
            {
                showTimer = true;
                cooldownTimeLeft = spellCooldown.CooldownLeft;
                cooldownTime = spellCooldown.Cooldown;
            }

            if (spellInfo.IsUsingCharges)
            {
                chargeText.SetCharArray(chargeCountText.SetIntNonAlloc(availableCharges, out var length), 0, length);
            }
            else
            {
                chargeText.SetCharArray(chargeCountText, 0, 0);
            }

            if (cooldownTimeLeft == 0)
            {
                cooldownText.SetCharArray(timerText, 0, 0);
                cooldownImage.fillAmount = 0;
                showingTimer = false;
            }
            else
            {
                if (showTimer)
                {
                    cooldownText.SetCharArray(timerText.SetSpellTimerNonAlloc(cooldownTimeLeft, out var length), 0, length);
                }
                else
                {
                    cooldownText.SetCharArray(timerText, 0, 0);
                }

                cooldownImage.fillAmount = (float)cooldownTimeLeft / cooldownTime;
                showingTimer = showTimer;
            }
        }
    }
}