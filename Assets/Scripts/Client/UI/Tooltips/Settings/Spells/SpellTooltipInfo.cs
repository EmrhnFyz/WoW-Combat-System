using Client.Localization;
using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Tooltip Info", menuName = "Game Data/Tooltips/Spell Tooltip Info", order = 1)]
    public sealed class SpellTooltipInfo : ScriptableUniqueInfo<SpellTooltipInfo>
    {
        [SerializeField] private SpellTooltipInfoContainer container;
        [SerializeField] private SpellInfo spellInfo;
        [SerializeField] private LocalizedString spellNameString;
        [SerializeField] private LocalizedString spellDescriptionString;
        [SerializeField] private List<SpellTooltipArgumentSettings> argumentSettings;

        protected override ScriptableUniqueInfoContainer<SpellTooltipInfo> Container => container;
        protected override SpellTooltipInfo Data => this;

        public const int MaxArguments = 5;

        public SpellInfo SpellInfo => spellInfo;
        public LocalizedString SpellNameString => spellNameString;
        public LocalizedString SpellDescriptionString => spellDescriptionString;
        public IReadOnlyList<SpellTooltipArgumentSettings> ArgumentSettings => argumentSettings;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (argumentSettings.Count > MaxArguments)
            {
                Debug.LogError($"Max argument amount of {MaxArguments} reached for {name}, increase size or decrease argument count!");
            }

            for (var i = 0; i < argumentSettings.Count; i++)
            {
                if (!argumentSettings[i].Validate())
                {
                    Debug.LogError($"Invalid tooltip argument №{i} for spell tooltip: {name}!");
                }
            }
        }
#endif
    }
}
