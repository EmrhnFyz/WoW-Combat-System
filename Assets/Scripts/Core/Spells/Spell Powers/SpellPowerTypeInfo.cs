using Common;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Spell Power Type Info", menuName = "Game Data/Spells/Power Type", order = 1)]
    public class SpellPowerTypeInfo : ScriptableUniqueInfo<SpellPowerTypeInfo>
    {
        [SerializeField] private SpellPowerTypeInfoContainer container;
        [SerializeField] private SpellPowerType powerType;
        [SerializeField] private EntityAttributes attributeTypeCurrent;
        [SerializeField] private EntityAttributes attributeTypeMax;
        [SerializeField] private EntityAttributes attributeTypeMaxNoMods;
        [SerializeField] private int minBasePower;
        [SerializeField] private int maxBasePower;
        [SerializeField] private int maxTotalPower;
        [SerializeField] private float regeneration;

        protected override SpellPowerTypeInfo Data => this;
        protected override ScriptableUniqueInfoContainer<SpellPowerTypeInfo> Container => container;

        public SpellPowerType PowerType => powerType;
        public EntityAttributes AttributeTypeCurrent => attributeTypeCurrent;
        public EntityAttributes AttributeTypeMax => attributeTypeMax;
        public EntityAttributes AttributeTypeMaxNoMods => attributeTypeMaxNoMods;
        public int MinBasePower => minBasePower;
        public int MaxBasePower => maxBasePower;
        public int MaxTotalPower => maxTotalPower;
        public float Regeneration => regeneration;
    }
}
