using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Invisibility", menuName = "Game Data/Spells/Auras/Effects/Invisibility", order = 3)]
    public class AuraEffectInfoInvisibility : AuraEffectInfo
    {
        [SerializeField, Range(0.0f, 100f)]
        private int invisibilityPower;

        public override float Value => invisibilityPower;
        public override AuraEffectType AuraEffectType => AuraEffectType.Invisibility;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectInvisiblity(aura, this, index, Value);
        }
    }
}