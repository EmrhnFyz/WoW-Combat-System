using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Modify Max Power", menuName = "Game Data/Spells/Auras/Effects/Modify Max Power", order = 3)]
    public class AuraEffectInfoModifyMaxPower : AuraEffectInfo
    {
        [SerializeField] private int power;
        [SerializeField] private SpellPowerType powerType;

        public override float Value => power;
        public override AuraEffectType AuraEffectType => AuraEffectType.ModifyMaxPower;
        public SpellPowerType PowerType => powerType;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectModifyMaxPower(aura, this, index, Value);
        }
    }
}