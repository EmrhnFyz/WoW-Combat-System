using Client.Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class BuffSlot : MonoBehaviour
    {
        [SerializeField] private RenderingReference rendering;
        [SerializeField] private Image contentImage;
        [SerializeField] private Image cooldownImage;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private TextMeshProUGUI chargesText;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private bool displayTimer;

        private readonly char[] timerText = { ' ', ' ', ' ' };
        private readonly char[] chargesCountText = { ' ', ' ', ' ' };
        private readonly char[] emptyTimerText = { ' ', ' ', ' ' };

        private IVisibleAura currentAura;

        public void UpdateAura(IVisibleAura visibleAura)
        {
            currentAura = visibleAura;

            if (currentAura == null || !visibleAura.HasActiveAura)
            {
                canvasGroup.alpha = 0.0f;
            }
            else
            {
                if (!displayTimer)
                {
                    cooldownText.SetCharArray(emptyTimerText, 0, 0);
                }

                if (currentAura.Charges > 1)
                {
                    chargesText.SetCharArray(chargesCountText.SetIntNonAlloc(currentAura.Charges, out var length), 0, length);
                }
                else
                {
                    chargesText.SetCharArray(emptyTimerText, 0, 0);
                }

                canvasGroup.alpha = 1.0f;
                contentImage.sprite = rendering.AuraVisuals.TryGetValue(visibleAura.AuraId, out AuraVisualsInfo settings)
                    ? settings.AuraIcon
                    : rendering.DefaultSpellIcon;
            }
        }

        public void DoUpdate()
        {
            if (currentAura == null)
            {
                return;
            }

            if (currentAura.MaxDuration == -1)
            {
                cooldownText.SetCharArray(timerText, 0, 0);
                cooldownImage.fillAmount = 0.0f;
            }
            else
            {
                if (displayTimer)
                {
                    if (currentAura.DurationLeft < 1000)
                    {
                        cooldownText.SetCharArray(timerText.SetSpellTimerNonAlloc(currentAura.DurationLeft, out var length), 0, length);
                    }
                    else
                    {
                        cooldownText.SetCharArray(emptyTimerText, 0, 0);
                    }
                }

                cooldownImage.fillAmount = 1.0f - ((float)currentAura.DurationLeft / currentAura.MaxDuration);
            }
        }
    }
}
