using Common;
using Core;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using EventHandler = Common.EventHandler;

namespace Client
{
    public class UnitFrame : MonoBehaviour
    {
        [SerializeField] private BalanceReference balance;
        [SerializeField] private RenderingReference rendering;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Image classIcon;
        [SerializeField] private AttributeBar health;
        [SerializeField] private AttributeBar mainResource;
        [SerializeField] private ComboFrame comboFrame;
        [SerializeField] private TextMeshProUGUI unitName;
        [SerializeField] private SoundEntry setSound;
        [SerializeField] private SoundEntry lostSound;

        private readonly Action<EntityAttributes> onAttributeChangedAction;
        private readonly Action onUnitTargetChanged;
        private readonly Action onUnitDisplayPowerChanged;
        private readonly Action onUnitClassChanged;

        private UnitFrame targetUnitFrame;
        private BuffDisplayFrame unitBuffDisplayFrame;
        private Unit unit;

        private UnitFrame()
        {
            onAttributeChangedAction = OnAttributeChanged;
            onUnitTargetChanged = OnUnitTargetChanged;
            onUnitDisplayPowerChanged = OnUnitDisplayPowerChanged;
            onUnitClassChanged = OnUnitClassChanged;
        }

        public void SetTargetUnitFrame(UnitFrame unitFrame)
        {
            targetUnitFrame = unitFrame;

            targetUnitFrame.UpdateUnit(unit != null ? unit.Target : null);
        }

        public void SetBuffDisplayFrame(BuffDisplayFrame buffDisplayFrame)
        {
            unitBuffDisplayFrame = buffDisplayFrame;

            unitBuffDisplayFrame.UpdateUnit(unit);
        }

        public void UpdateUnit(Unit newUnit)
        {
            var wasSet = unit != null;

            if (unit != null)
            {
                DeinitializeUnit();
            }

            if (newUnit != null)
            {
                InitializeUnit(newUnit);
            }

            if (unit != null && setSound != null)
            {
                setSound.Play();
            }
            else if (wasSet && lostSound != null)
            {
                lostSound.Play();
            }

            canvasGroup.blocksRaycasts = unit != null;
            canvasGroup.interactable = unit != null;
            canvasGroup.alpha = unit != null ? 1.0f : 0.0f;
        }

        private void InitializeUnit(Unit unit)
        {
            this.unit = unit;
            unitName.text = unit.Name;

            if (comboFrame != null)
            {
                comboFrame.UpdateUnit(unit);
            }

            if (targetUnitFrame != null)
            {
                targetUnitFrame.UpdateUnit(unit.Target);
            }

            if (unitBuffDisplayFrame)
            {
                unitBuffDisplayFrame.UpdateUnit(unit);
            }

            OnAttributeChanged(EntityAttributes.Health);
            OnAttributeChanged(EntityAttributes.Power);
            OnUnitClassChanged();
            OnUnitDisplayPowerChanged();

            EventHandler.SubscribeEvent(unit, GameEvents.UnitAttributeChanged, onAttributeChangedAction);
            EventHandler.SubscribeEvent(unit, GameEvents.UnitTargetChanged, onUnitTargetChanged);
            EventHandler.SubscribeEvent(unit, GameEvents.UnitClassChanged, onUnitClassChanged);
            EventHandler.SubscribeEvent(unit, GameEvents.UnitDisplayPowerChanged, onUnitDisplayPowerChanged);
        }

        private void DeinitializeUnit()
        {
            EventHandler.UnsubscribeEvent(unit, GameEvents.UnitAttributeChanged, onAttributeChangedAction);
            EventHandler.UnsubscribeEvent(unit, GameEvents.UnitTargetChanged, onUnitTargetChanged);
            EventHandler.UnsubscribeEvent(unit, GameEvents.UnitClassChanged, onUnitClassChanged);
            EventHandler.UnsubscribeEvent(unit, GameEvents.UnitDisplayPowerChanged, onUnitDisplayPowerChanged);

            if (comboFrame != null)
            {
                comboFrame.UpdateUnit(null);
            }

            if (targetUnitFrame != null)
            {
                targetUnitFrame.UpdateUnit(null);
            }

            if (unitBuffDisplayFrame)
            {
                unitBuffDisplayFrame.UpdateUnit(null);
            }

            unit = null;
        }

        private void OnAttributeChanged(EntityAttributes attributeType)
        {
            if (attributeType is EntityAttributes.Health or EntityAttributes.MaxHealth)
            {
                health.Ratio = unit.HealthRatio;
            }
            else if (attributeType is EntityAttributes.Power or EntityAttributes.MaxPower)
            {
                mainResource.Ratio = Mathf.Clamp01((float)unit.Power / unit.MaxPower);
            }
        }

        private void OnUnitTargetChanged()
        {
            if (targetUnitFrame != null)
            {
                targetUnitFrame.UpdateUnit(unit.Target);
            }
        }

        private void OnUnitDisplayPowerChanged()
        {
            mainResource.FillImage.color = rendering.SpellPowerColors.Value(unit.DisplayPowerType);
        }

        private void OnUnitClassChanged()
        {
            classIcon.sprite = rendering.ClassIconSprites.Value(unit.ClassType);
            if (comboFrame != null && balance.ClassesByType.TryGetValue(unit.ClassType, out ClassInfo classInfo))
            {
                comboFrame.Canvas.enabled = classInfo.HasPower(SpellPowerType.ComboPoints);
            }
        }
    }
}