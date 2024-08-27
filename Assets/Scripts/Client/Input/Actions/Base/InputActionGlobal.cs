using Common;
using Core.Conditions;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Input Action Global", menuName = "Player Data/Input/Input Action Global", order = 1)]
    public class InputActionGlobal : ScriptableObject
    {
        [SerializeField] private InputReference input;
        [SerializeField] private InputAction action;
        [SerializeField] private HotkeyInputItem hotkey;
        [SerializeField] private List<InputActionGlobal> blockedByActions;
        [SerializeField] private List<Condition> blockInactiveWhen;
        [SerializeField] private List<Condition> hotkeyInactiveWhen;

        private bool IsBlockApplicable
        {
            get
            {
                foreach (Condition condition in blockInactiveWhen)
                {
                    if (condition.IsApplicableAndValid(input.Player))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private bool IsHotkeyApplicable
        {
            get
            {
                foreach (Condition condition in hotkeyInactiveWhen)
                {
                    if (condition.IsApplicableAndValid(input.Player))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void Register()
        {
            EventHandler.SubscribeEvent<HotkeyState>(hotkey, GameEvents.HotkeyStateChanged, OnHotkeyStateChanged);
        }

        public void Unregister()
        {
            EventHandler.UnsubscribeEvent<HotkeyState>(hotkey, GameEvents.HotkeyStateChanged, OnHotkeyStateChanged);
        }

        private void OnHotkeyStateChanged(HotkeyState state)
        {
            if (state == HotkeyState.Released || blockedByActions.Exists(blocker => blocker.IsBlockApplicable))
            {
                return;
            }

            if (IsHotkeyApplicable)
            {
                action.Execute();
            }
        }
    }
}
