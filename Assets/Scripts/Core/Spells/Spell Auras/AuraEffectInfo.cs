using UnityEngine;

namespace Core
{
    public abstract class AuraEffectInfo : ScriptableObject
    {
        [Header("Aura Effect")]
        [SerializeField] private int maxEffectiveCharges;
        [SerializeField] private SpellMechanics mechanics;

        public abstract float Value { get; }
        public abstract AuraEffectType AuraEffectType { get; }
        public virtual float SecondaryValue => Value;

        public SpellMechanics Mechanics => mechanics;

        internal abstract AuraEffect CreateEffect(Aura aura, Unit caster, int index);

        internal bool IsAffectingSpell(SpellInfo spellInfo) => true;
    }
}
