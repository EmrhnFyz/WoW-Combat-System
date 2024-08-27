using Client.Localization;
using Common;
using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Localization Reference", menuName = "Game Data/Scriptable/Localization", order = 2)]
    public partial class LocalizationReference : Localization.LocalizationReference
    {
        [SerializeField] private LocalizedString missingStringPlaceholder;
        [SerializeField] private LocalizedString emptyStringPlaceholder;
        [SerializeField] private SpellTooltipInfoContainer spellTooltipSettings;
        [SerializeField] private List<HotKeyModifierLink> hotkeyModifiers;
        [SerializeField] private List<KeyCodeLink> keyCodes;
        [SerializeField] private List<SpellCastResultLink> spellCastResults;
        [SerializeField] private List<SpellMissTypeLink> spellMissTypes;
        [SerializeField] private List<ClientConnectFailReasonLink> clientConnectFailReasons;
        [SerializeField] private List<PowerTypeCostLink> powerTypeCosts;

        private static readonly Dictionary<KeyCode, string> StringsByKeyCode = new();
        private static readonly Dictionary<HotkeyModifier, string> StringsByHotkeyModifier = new();
        private static readonly Dictionary<SpellCastResult, LocalizedString> StringsBySpellCastResult = new();
        private static readonly Dictionary<SpellMissType, LocalizedString> StringsBySpellMissType = new();
        private static readonly Dictionary<ClientConnectFailReason, LocalizedString> StringsByClientConnectFailReason = new();
        private static readonly Dictionary<SpellPowerType, PowerTypeCostLink> StringsBySpellPowerType = new();

        private static LocalizedString MissingString;
        private static LocalizedString EmptyString;

        public IReadOnlyDictionary<SpellInfo, SpellTooltipInfo> TooltipInfoBySpell => spellTooltipSettings.TooltipInfoBySpell;
        public IReadOnlyDictionary<int, SpellTooltipInfo> TooltipInfoBySpellId => spellTooltipSettings.TooltipInfoBySpellId;

        protected override void OnRegistered()
        {
            base.OnRegistered();

            MissingString = missingStringPlaceholder;
            EmptyString = emptyStringPlaceholder;

            spellTooltipSettings.Register();
            keyCodes.ForEach(item => StringsByKeyCode.Add(item.KeyCode, item.String));
            hotkeyModifiers.ForEach(item => StringsByHotkeyModifier.Add(item.Modifier, item.String));
            spellCastResults.ForEach(item => StringsBySpellCastResult.Add(item.SpellCastResult, item.LocalizedString));
            spellMissTypes.ForEach(item => StringsBySpellMissType.Add(item.SpellMissType, item.LocalizedString));
            clientConnectFailReasons.ForEach(item => StringsByClientConnectFailReason.Add(item.FailReason, item.LocalizedString));
            powerTypeCosts.ForEach(item => StringsBySpellPowerType.Add(item.PowerType, item));

            foreach (KeyCode item in Enum.GetValues(typeof(KeyCode)))
            {
                if (!StringsByKeyCode.ContainsKey(item))
                {
                    StringsByKeyCode[item] = item.ToString();
                }
            }

            foreach (HotkeyModifier item in Enum.GetValues(typeof(HotkeyModifier)))
            {
                if (!StringsByHotkeyModifier.ContainsKey(item))
                {
                    StringsByHotkeyModifier[item] = item.ToString();
                }
            }
        }

        protected override void OnUnregister()
        {
            StringsByKeyCode.Clear();
            StringsByHotkeyModifier.Clear();
            StringsBySpellCastResult.Clear();
            StringsByClientConnectFailReason.Clear();
            StringsBySpellPowerType.Clear();
            spellTooltipSettings.Unregister();

            MissingString = null;
            EmptyString = null;

            base.OnUnregister();
        }

        public static LocalizedString Localize(SpellCastResult castResult)
        {
            Assert.IsTrue(StringsBySpellCastResult.ContainsKey(castResult), $"Missing localization for SpellCastResult: {castResult}");

            if (StringsBySpellCastResult.TryGetValue(castResult, out LocalizedString localizedString))
            {
                return localizedString;
            }

            return MissingString;
        }

        public static LocalizedString Localize(SpellMissType spellMissType)
        {
            if (StringsBySpellMissType.TryGetValue(spellMissType, out LocalizedString localizedString))
            {
                return localizedString;
            }

            return EmptyString;
        }

        public static LocalizedString Localize(ClientConnectFailReason failReason)
        {
            Assert.IsTrue(StringsByClientConnectFailReason.ContainsKey(failReason), $"Missing localization for ClientConnectFailReason: {failReason}");

            if (StringsByClientConnectFailReason.TryGetValue(failReason, out LocalizedString localizedString))
            {
                return localizedString;
            }

            return MissingString;
        }

        public static LocalizedString Localize(SpellPowerType powerType, bool isPercentage)
        {
            Assert.IsTrue(StringsBySpellPowerType.ContainsKey(powerType), $"Missing localization for PowerType: {powerType}");

            if (StringsBySpellPowerType.TryGetValue(powerType, out PowerTypeCostLink powerTypeEntry))
            {
                return isPercentage ? powerTypeEntry.LocalizedPercentageString : powerTypeEntry.LocalizedRawString;
            }

            return MissingString;
        }

        public static string Localize(HotkeyInputItem hotkeyInput)
        {
            if (hotkeyInput.KeyCode == KeyCode.None)
            {
                return string.Empty;
            }

            var result = string.Empty;
            if (hotkeyInput.Modifier != HotkeyModifier.None)
            {
                result = $"{StringsByHotkeyModifier[hotkeyInput.Modifier]}-";
            }

            return $"{result}{StringsByKeyCode[hotkeyInput.KeyCode]}";
        }
    }
}
