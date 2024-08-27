using Core.Conditions;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Spell Modifier", menuName = "Game Data/Spells/Auras/Effects/Spell Modifier", order = 2)]
    public class AuraEffectInfoSpellModifier : AuraEffectInfo
    {
        [SerializeField] private float modifierValue;
        [SerializeField] private SpellModifierType modifierType;
        [SerializeField] private SpellModifierApplicationType applicationType;
        [SerializeField] private SpellValueModifier spellValueModifier;
        [SerializeField] private List<Condition> applicationConditions;

        public SpellModifierType ModifierType => modifierType;
        public SpellModifierApplicationType ApplicationType => applicationType;
        public SpellValueModifier SpellValueModifier => spellValueModifier;
        public IReadOnlyList<Condition> ApplicationConditions => applicationConditions;

        public override float Value => modifierValue;
        public override AuraEffectType AuraEffectType => AuraEffectType.SpeedIncreaseModifier;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectSpellModifier(aura, this, index, Value);
        }
    }
}