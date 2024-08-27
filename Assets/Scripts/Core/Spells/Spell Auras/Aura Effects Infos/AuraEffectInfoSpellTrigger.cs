using Core.Conditions;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Spell Trigger", menuName = "Game Data/Spells/Auras/Effects/Spell Trigger", order = 4)]
    public class AuraEffectInfoSpellTrigger : AuraEffectInfo
    {
        [SerializeField, Range(0.0f, 1.0f)] private float chance;
        [SerializeField, Range(0.0f, 1.0f)] private float chancePerCombo;
        [SerializeField] private bool isCasterTriggerTarget;
        [SerializeField] private bool canCasterBeTriggerTarget;
        [SerializeField] private SpellInfo triggeredSpell;
        [SerializeField] private SpellTriggerFlags triggerFlags;
        [SerializeField] private List<Condition> triggerConditions;

        public bool IsCasterTriggerTarget => isCasterTriggerTarget;
        public bool CanCasterBeTriggerTarget => canCasterBeTriggerTarget;
        public float Chance => chance;
        public float ChancePerCombo => chancePerCombo;
        public SpellInfo TriggeredSpell => triggeredSpell;
        public SpellTriggerFlags TriggerFlags => triggerFlags;
        public IReadOnlyList<Condition> TriggerConditions => triggerConditions;

        public override float Value => chance;
        public override AuraEffectType AuraEffectType => AuraEffectType.TriggerSpellChance;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectSpellTrigger(aura, this, index, Value);
        }
    }
}