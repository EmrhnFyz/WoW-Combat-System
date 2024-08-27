using Common;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    public class ButtonSlot : UIBehaviour, IPointerDownHandler, IDropHandler
    {
        [SerializeField] private HotkeyInputItem hotkeyInput;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private ButtonContent buttonContent;
        [SerializeField] private SoundEntry pressSound;
        [SerializeField] private TextMeshProUGUI hotkeyText;

        public RectTransform RectTransform => rectTransform;
        public ButtonContent ButtonContent => buttonContent;

        public void Initialize()
        {
            buttonContent.Initialize(this);

            EventHandler.SubscribeEvent<HotkeyState>(hotkeyInput, GameEvents.HotkeyStateChanged, OnHotkeyStateChanged);
            EventHandler.SubscribeEvent(hotkeyInput, GameEvents.HotkeyBindingChanged, OnHotkeyBindingChanged);

            OnHotkeyBindingChanged();
        }

        public void Denitialize()
        {
            EventHandler.UnsubscribeEvent<HotkeyState>(hotkeyInput, GameEvents.HotkeyStateChanged, OnHotkeyStateChanged);
            EventHandler.UnsubscribeEvent(hotkeyInput, GameEvents.HotkeyBindingChanged, OnHotkeyBindingChanged);

            buttonContent.Deinitialize();
        }

        public void DoUpdate()
        {
            buttonContent.DoUpdate();
        }

        [Description("Also called from manually pressing button.")]
        public void Click()
        {
            if (!buttonContent.IsAlreadyPressed)
            {
                pressSound?.Play();
                buttonContent.Activate();
            }
        }

        public void OnPointerDown(PointerEventData data)
        {
        }

        public void OnDrop(PointerEventData data)
        {
        }

        private void OnHotkeyStateChanged(HotkeyState state)
        {
            if (state == HotkeyState.Pressed)
            {
                Click();
            }

            buttonContent.HandleHotkeyState(state);
        }

        private void OnHotkeyBindingChanged()
        {
            hotkeyText.text = LocalizationReference.Localize(hotkeyInput);
        }
    }
}