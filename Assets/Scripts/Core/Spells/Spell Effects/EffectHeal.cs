using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Effect Heal", menuName = "Game Data/Spells/Effects/Heal", order = 5)]
    public class EffectHeal : SpellEffectInfo
    {
        [Header("Heal")]
        [SerializeField] private int baseValue;
        [SerializeField] private uint baseVariance;
        [SerializeField] private uint additionalValue;
        [SerializeField] private SpellDamageCalculationType calculationType;
        [SerializeField] private List<ConditionalModifier> conditionalModifiers;

        public override float Value => baseValue;
        public override SpellEffectType EffectType => SpellEffectType.Heal;
        public IReadOnlyList<ConditionalModifier> ConditionalModifiers => conditionalModifiers;

        internal override void Handle(Spell spell, int effectIndex, Unit target, SpellEffectHandleMode mode)
        {
            spell.EffectHeal(this, effectIndex, target, mode);
        }

        internal int CalculateSpellHeal(SpellInfo spellInfo, int effectIndex, Unit caster = null, Unit target = null)
        {
            var rolledValue = RandomUtils.Next(baseValue, (int)(baseValue + baseVariance));
            float baseHeal = 0;

            switch (calculationType)
            {
                case SpellDamageCalculationType.Direct:
                    baseHeal = rolledValue + additionalValue;
                    break;
                case SpellDamageCalculationType.SpellPowerPercent:
                    if (caster == null)
                    {
                        break;
                    }

                    baseHeal = additionalValue + caster.SpellPower.ApplyPercentage(rolledValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculationType), $"Unknown healing calculation type: {calculationType}");
            }

            if (caster != null)
            {
                baseHeal = caster.Spells.ApplyEffectModifiers(spellInfo, baseHeal);
            }

            return (int)baseHeal;
        }
    }

    public partial class Spell
    {
        internal void EffectHeal(EffectHeal effect, int effectIndex, Unit target, SpellEffectHandleMode mode)
        {
            if (mode != SpellEffectHandleMode.HitStart || !target.IsAlive)
            {
                return;
            }

            float spellHealAmount = effect.CalculateSpellHeal(SpellInfo, effectIndex, Caster, target);
            for (var i = 0; i < effect.ConditionalModifiers.Count; i++)
            {
                ConditionalModifier modifier = effect.ConditionalModifiers[i];
                if (modifier.Condition.IsApplicableAndValid(Caster, target, this))
                {
                    modifier.Modify(Caster, target, ref spellHealAmount);
                }
            }

            EffectHealing += (int)spellHealAmount;
        }
    }
}
