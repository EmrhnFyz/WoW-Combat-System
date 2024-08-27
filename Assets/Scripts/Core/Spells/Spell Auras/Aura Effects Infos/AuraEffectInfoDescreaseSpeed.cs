using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Decrease Speed", menuName = "Game Data/Spells/Auras/Effects/Speed Decrease", order = 1)]
    public class AuraEffectInfoDescreaseSpeed : AuraEffectInfo
    {
        [SerializeField, Range(1.0f, 99.9f)] private float decreasePercent;

        public override float Value => decreasePercent;
        public override AuraEffectType AuraEffectType => AuraEffectType.SpeedDecreaseModifier;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectSharedSpeedChange(aura, this, index, Value);
        }
    }
}