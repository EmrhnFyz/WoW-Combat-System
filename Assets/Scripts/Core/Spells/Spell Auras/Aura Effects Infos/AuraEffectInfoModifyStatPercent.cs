using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Modify Stat Percent", menuName = "Game Data/Spells/Auras/Effects/Modify Stat Percent", order = 3)]
    public class AuraEffectInfoModifyStatPercent : AuraEffectInfo
    {
        [SerializeField, Range(-99.99f, 500f)]
        private float statPercent;
        [SerializeField]
        private StatType statType;

        public override float Value => statPercent;
        public override AuraEffectType AuraEffectType => AuraEffectType.ModifyStatPercent;
        public StatType StatType => statType;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectModifyStatPercent(aura, this, index, Value);
        }
    }
}