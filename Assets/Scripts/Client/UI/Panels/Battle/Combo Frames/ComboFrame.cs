using Common;
using Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = Common.EventHandler;

namespace Client
{
    public class ComboFrame : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private List<ComboPointSlot> comboPointSlots;

        private readonly Action<EntityAttributes> onAttributeChangedAction;

        private Unit unit;

        private ComboFrame() => onAttributeChangedAction = OnAttributeChanged;

        public Canvas Canvas => canvas;

        public void UpdateUnit(Unit newUnit)
        {
            if (unit != null)
            {
                UnregisterUnit();
            }

            if (newUnit != null)
            {
                RegisterUnit(newUnit);
            }

            canvasGroup.blocksRaycasts = unit != null;
            canvasGroup.interactable = unit != null;
            canvasGroup.alpha = unit != null ? 1.0f : 0.0f;
        }

        private void RegisterUnit(Unit unit)
        {
            this.unit = unit;

            OnAttributeChanged(EntityAttributes.ComboPoints);

            EventHandler.SubscribeEvent(unit, GameEvents.UnitAttributeChanged, onAttributeChangedAction);
        }

        private void UnregisterUnit()
        {
            EventHandler.UnsubscribeEvent(unit, GameEvents.UnitAttributeChanged, onAttributeChangedAction);

            unit = null;
        }

        private void OnAttributeChanged(EntityAttributes attributeType)
        {
            if (attributeType == EntityAttributes.ComboPoints)
            {
                for (var i = 0; i < comboPointSlots.Count; i++)
                {
                    comboPointSlots[i].ModifyState(i < unit.ComboPoints);
                }
            }
        }
    }
}