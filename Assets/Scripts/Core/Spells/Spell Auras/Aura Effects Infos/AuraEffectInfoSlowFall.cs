using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Slow Fall", menuName = "Game Data/Spells/Auras/Effects/Slow Fall", order = 3)]
    public class AuraEffectInfoSlowFall : AuraEffectInfo
    {
        [SerializeField, Range(0, 7)] private int slowFallSpeed;

        public override float Value => slowFallSpeed;
        public override AuraEffectType AuraEffectType => AuraEffectType.SlowFall;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectSlowFall(aura, this, index, Value);
        }
    }
}