using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Modify Power Regen Percent", menuName = "Game Data/Spells/Auras/Effects/Modify Power Regen Percent", order = 3)]
    public class AuraEffectInfoModifyPowerRegenPercent : AuraEffectInfo
    {
        [SerializeField, Range(-100f, 500f)] private float powerRegenPercent;
        [SerializeField] private SpellPowerType powerType;

        public override float Value => powerRegenPercent;
        public override float SecondaryValue => (int)powerType;
        public override AuraEffectType AuraEffectType => AuraEffectType.ModifyPowerRegenPercent;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectSharedBasicModifer(aura, this, index, Value);
        }
    }
}