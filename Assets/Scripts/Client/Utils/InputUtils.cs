using System;
using UnityEngine;

namespace Client
{
    public static class InputUtils
    {
        private static readonly KeyCode[] HotkeyModifiers = { KeyCode.LeftAlt, KeyCode.LeftControl, KeyCode.LeftShift };

        public const int ActionBarSlotCount = 14;
        public const int ActionBarCount = 6;

        public static KeyCode ToKeyCode(this HotkeyModifier hotkeyModifier)
        {
            return hotkeyModifier switch
            {
                HotkeyModifier.None => KeyCode.None,
                HotkeyModifier.LeftControl => KeyCode.LeftControl,
                HotkeyModifier.LeftAlt => KeyCode.LeftAlt,
                HotkeyModifier.LeftShift => KeyCode.LeftShift,
                _ => throw new ArgumentOutOfRangeException(nameof(hotkeyModifier)),
            };
        }

        public static bool AnyHotkeyModifiersPressedExcept(KeyCode modifier)
        {
            for (var i = 0; i < HotkeyModifiers.Length; i++)
            {
                if (HotkeyModifiers[i] != modifier && Input.GetKey(HotkeyModifiers[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasTargetFlag(this TargetingEntityType entityTypes, TargetingEntityType targetingEntityType)
        {
            return (entityTypes & targetingEntityType) == targetingEntityType;
        }
    }
}
