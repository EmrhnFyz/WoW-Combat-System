using Client.Localization;
using Common;
using Core;
using TMPro;
using UnityEngine;

namespace Client
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro textMesh;
        [SerializeField] private CameraReference cameraReference;
        [SerializeField] private FloatingTextSettings damageSettings;
        [SerializeField] private FloatingTextSettings damageCritSettings;
        [SerializeField] private FloatingTextSettings fullAbsorbSettings;
        [SerializeField] private FloatingTextSettings missSettings;
        [SerializeField] private FloatingTextSettings healingSettings;
        [SerializeField] private FloatingTextSettings healingCritSettings;
        [SerializeField] private LocalizedString fullAbsrobString;

        private float currentLifeTime;
        private float targetLifeTime;
        private FloatingTextSettings currentSettings;

        private void OnDestroy()
        {
            GameObjectPool.Return(this, true);
        }

        public void SetMissText(SpellMissType missType)
        {
            SetText(missSettings, LocalizationReference.Localize(missType).Value);
        }

        public void SetDamage(int damageAmount, HitType hitType)
        {
            if (hitType == HitType.Immune)
            {
                SetText(missSettings, LocalizationReference.Localize(SpellMissType.Immune).Value);
            }
            else if (hitType.HasTargetFlag(HitType.FullAbsorb))
            {
                SetText(fullAbsorbSettings, fullAbsrobString.Value);
            }
            else
            {
                SetText(hitType.HasTargetFlag(HitType.CriticalHit) ? damageCritSettings : damageSettings, damageAmount.ToString());
            }
        }

        public void SetHealing(int healingAmount, bool isCrit)
        {
            SetText(isCrit ? healingCritSettings : healingSettings, healingAmount.ToString());
        }

        public bool DoUpdate(float deltaTime)
        {
            currentLifeTime += deltaTime;

            transform.localScale = Vector3.one * currentSettings.SizeOverTime.Evaluate(currentLifeTime);
            transform.Translate(currentSettings.FloatingSpeed * deltaTime * Vector3.up);

            WarcraftCamera warcraftCamera = cameraReference.WarcraftCamera;
            if (warcraftCamera != null)
            {
                Vector3 direction = transform.position - warcraftCamera.transform.position;
                var distance = Vector3.Dot(direction, warcraftCamera.transform.forward);

                transform.rotation = Quaternion.LookRotation(warcraftCamera.transform.forward);
                transform.localScale *= currentSettings.SizeOverDistanceToCamera.Evaluate(distance);
            }

            textMesh.alpha = currentSettings.AlphaOverTime.Evaluate(currentLifeTime);
            return currentLifeTime >= targetLifeTime;
        }

        private void SetText(FloatingTextSettings newSettings, string value)
        {
            currentSettings = newSettings;
            textMesh.text = value;
            textMesh.fontSharedMaterial = newSettings.FontMaterial;
            textMesh.fontSize = currentSettings.FontSize;
            textMesh.color = currentSettings.FontColor;
            targetLifeTime = currentSettings.LifeTime;
            transform.localScale = Vector3.one;
            currentLifeTime = 0;

            WarcraftCamera warcraftCamera = cameraReference.WarcraftCamera;
            if (warcraftCamera != null)
            {
                Vector3 direction = transform.position - warcraftCamera.transform.position;
                var distance = Vector3.Dot(direction, warcraftCamera.transform.forward);
                transform.position += currentSettings.RandomOffset * currentSettings.RandomOffsetOverDistance.Evaluate(distance) * Random.insideUnitSphere;
            }
            else
            {
                transform.position += Random.insideUnitSphere * currentSettings.RandomOffset;
            }
        }
    }
}
