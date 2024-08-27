using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Periodic Healing", menuName = "Game Data/Spells/Auras/Effects/Periodic Healing", order = 2)]
    public class AuraEffectInfoPeriodicHealing : AuraEffectInfoPeriodic
    {
        [SerializeField] private int baseValue;
        [SerializeField] private uint additionalValue;
        [SerializeField] private SpellDamageCalculationType calculationType;
        [SerializeField] private List<ConditionalModifier> conditionalModifiers;

        public override float Value => baseValue;
        public override AuraEffectType AuraEffectType => AuraEffectType.PeriodicHealing;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectPeriodicHealing(aura, this, index, Value);
        }

        internal int CalculateSpellHeal(Unit caster)
        {
            float baseHeal = 0;

            switch (calculationType)
            {
                case SpellDamageCalculationType.Direct:
                    baseHeal = baseValue + additionalValue;
                    break;
                case SpellDamageCalculationType.SpellPowerPercent:
                    if (caster == null)
                    {
                        break;
                    }

                    baseHeal = additionalValue + caster.SpellPower.ApplyPercentage(baseValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculationType), $"Unknown healing calculation type: {calculationType}");
            }

            return (int)baseHeal;
        }
    }
}