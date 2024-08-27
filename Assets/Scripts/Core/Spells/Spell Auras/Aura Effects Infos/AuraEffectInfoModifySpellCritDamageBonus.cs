using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Modify Crit Damage Bonus", menuName = "Game Data/Spells/Auras/Effects/Modify Crit Damage Bonus", order = 3)]
    public class AuraEffectInfoModifySpellCritDamageBonus : AuraEffectInfo
    {
        [SerializeField, Range(-100f, 500f)] private float critDamagePercentageMultiplier;

        public override float Value => critDamagePercentageMultiplier;
        public override AuraEffectType AuraEffectType => AuraEffectType.ModifyCritDamageBonus;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectSharedBasicModifer(aura, this, index, Value);
        }
    }
}