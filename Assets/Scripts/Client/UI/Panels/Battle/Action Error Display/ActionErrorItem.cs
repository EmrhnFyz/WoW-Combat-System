using Client.Localization;
using Common;
using Core;
using UnityEngine;

namespace Client
{
    public class ActionErrorItem : MonoBehaviour
    {
        [SerializeField] private LocalizedTextMeshProUGUI errorLabel;
        [SerializeField] private ActionErrorDisplaySettings displaySettings;
        [SerializeField] private RectTransform rectTransform;

        private float currentLifeTime;
        private float targetLifeTime;

        public SpellCastResult CastResult { get; private set; }
        public RectTransform RectTransform => rectTransform;

        private void OnDestroy()
        {
            GameObjectPool.Return(this, true);
        }

        public void SetErrorText(SpellCastResult spellCastResult)
        {
            CastResult = spellCastResult;

            errorLabel.SetString(LocalizationReference.Localize(spellCastResult));
            errorLabel.TextMeshPro.fontSize = displaySettings.FontSize;
            targetLifeTime = displaySettings.LifeTime;
            transform.localScale = Vector3.one;
            currentLifeTime = 0;
        }

        public bool DoUpdate(float deltaTime)
        {
            currentLifeTime += deltaTime;

            rectTransform.localScale = Vector3.one * displaySettings.SizeOverTime.Evaluate(currentLifeTime);
            rectTransform.Translate(deltaTime * displaySettings.FloatingSpeed * Vector3.up);

            Color baseColor = errorLabel.TextMeshPro.color;
            errorLabel.TextMeshPro.color = new Color(baseColor.r, baseColor.g, baseColor.b, displaySettings.AlphaOverTime.Evaluate(currentLifeTime));
            return currentLifeTime >= targetLifeTime;
        }
    }
}