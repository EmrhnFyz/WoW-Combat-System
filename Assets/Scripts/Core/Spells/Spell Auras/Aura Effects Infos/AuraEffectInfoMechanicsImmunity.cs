using System.Collections.Generic;
using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Mechanics Immunity", menuName = "Game Data/Spells/Auras/Effects/Mechanics Immunity", order = 4)]
    public class AuraEffectInfoMechanicsImmunity : AuraEffectInfo
    {
        [SerializeField] private List<SpellMechanics> immuneMechanics;

        public override float Value => 1.0f;
        public override AuraEffectType AuraEffectType => AuraEffectType.MechanicImmunity;
        public IReadOnlyList<SpellMechanics> ImmuneMechanics => immuneMechanics;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectMechanicsImmunity(aura, this, index, Value);
        }
    }
}