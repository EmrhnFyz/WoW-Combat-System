using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.AuraEffects
{
    [CreateAssetMenu(fileName = "Aura Effect Periodic Damage", menuName = "Game Data/Spells/Auras/Effects/Periodic Damage", order = 1)]
    public class AuraEffectInfoPeriodicDamage : AuraEffectInfoPeriodic
    {
        [SerializeField] private int baseValue;
        [SerializeField] private uint additionalValue;
        [SerializeField] private SpellSchoolMask schoolMask;
        [SerializeField] private SpellDamageCalculationType calculationType;
        [SerializeField] private List<ConditionalModifier> conditionalModifiers;

        public SpellSchoolMask SpellSchoolMask => schoolMask;
        public override float Value => baseValue;
        public override AuraEffectType AuraEffectType => AuraEffectType.PeriodicDamage;

        internal override AuraEffect CreateEffect(Aura aura, Unit caster, int index)
        {
            return new AuraEffectPeriodicDamage(aura, this, index, Value);
        }

        internal int CalculateSpellDamage(Unit caster)
        {
            float baseDamage = 0;

            switch (calculationType)
            {
                case SpellDamageCalculationType.Direct:
                    baseDamage = baseValue + additionalValue;
                    break;
                case SpellDamageCalculationType.SpellPowerPercent:
                    if (caster == null)
                    {
                        break;
                    }

                    baseDamage = additionalValue + caster.SpellPower.ApplyPercentage(baseValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(calculationType), $"Unknown damage calculation type: {calculationType}");
            }

            return (int)baseDamage;
        }
    }
}