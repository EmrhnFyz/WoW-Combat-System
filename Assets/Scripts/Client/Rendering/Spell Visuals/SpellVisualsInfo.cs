using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client.Spells
{
    [CreateAssetMenu(fileName = "Spell Visual Info", menuName = "Game Data/Visuals/Spell Visual Info", order = 2)]
    public class SpellVisualsInfo : ScriptableUniqueInfo<SpellVisualsInfo>
    {
        [SerializeField] private SpellVisualsInfoContainer container;
        [SerializeField] private SpellInfo spellInfo;
        [SerializeField] private Sprite spellIcon;
        [SerializeField] private List<EffectSpellSettings> visualEffects = new();

        private readonly Dictionary<EffectSpellSettings.UsageType, EffectSpellSettings> visualsByUsage = new();

        protected override SpellVisualsInfo Data => this;
        protected override ScriptableUniqueInfoContainer<SpellVisualsInfo> Container => container;

        public SpellInfo SpellInfo => spellInfo;
        public Sprite SpellIcon => spellIcon;
        public IReadOnlyDictionary<EffectSpellSettings.UsageType, EffectSpellSettings> VisualsByUsage => visualsByUsage;

        public void Initialize()
        {
            visualEffects.ForEach(effect => visualsByUsage.Add(effect.VisualUsageType, effect));
        }

        public void Deinitialize()
        {
            visualsByUsage.Clear();
        }
    }
}

