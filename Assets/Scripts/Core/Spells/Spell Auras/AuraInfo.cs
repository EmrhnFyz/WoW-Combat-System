using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Aura Info", menuName = "Game Data/Spells/Auras/Aura Info", order = 1)]
    public sealed class AuraInfo : ScriptableUniqueInfo<AuraInfo>
    {
        [SerializeField] private AuraInfoContainer container;

        [Header("Aura Info")]
        [SerializeField] private int duration;
        [SerializeField] private int maxDuration;
        [SerializeField] private int maxStack;
        [SerializeField] private AuraStateType stateType;
        [SerializeField, EnumFlag] private AuraTargetingMode targetingMode;
        [SerializeField, EnumFlag] private AuraInterruptFlags interruptFlags;
        [SerializeField, EnumFlag] private AuraAttributes attributes;
        [SerializeField] private List<AuraEffectInfo> auraEffects;
        [SerializeField] private List<AuraScriptable> auraScriptables;

        [Header("Charges")]
        [SerializeField] private bool usesCharges;
        [SerializeField] private int maxCharges;
        [SerializeField] private int baseCharges;

        [Header("Damage Interrupt Info")]
        [SerializeField] private int damageInterruptValue;
        [SerializeField] private int damageInterruptDelay;
        [SerializeField] private AuraInterruptValueCalculationType interruptValueType;

        protected override ScriptableUniqueInfoContainer<AuraInfo> Container => container;
        protected override AuraInfo Data => this;

        public new int Id => base.Id;
        public int Charges => baseCharges;
        public int MaxCharges => maxCharges;
        public int Duration => duration;
        public int MaxDuration => maxDuration;
        public bool UsesCharges => usesCharges;
        public bool HasInterruptFlags => interruptFlags != 0;
        public AuraStateType StateType => stateType;
        public AuraTargetingMode TargetingMode => targetingMode;
        public AuraInterruptFlags InterruptFlags => interruptFlags;
        public IReadOnlyList<AuraEffectInfo> AuraEffects => auraEffects;

        internal IReadOnlyList<AuraScriptable> AuraScriptables => auraScriptables;

        public bool IsPositive => !HasAttribute(AuraAttributes.Negative);

        public bool HasAttribute(AuraAttributes attribute)
        {
            return (attributes & attribute) != 0;
        }

        public bool HasMechanics(SpellMechanics mechanics)
        {
            foreach (AuraEffectInfo auraEffectInfo in auraEffects)
            {
                if (auraEffectInfo.Mechanics == mechanics)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAnyMechanics(SpellMechanicsFlags mechanicsFlags)
        {
            foreach (AuraEffectInfo auraEffectInfo in auraEffects)
            {
                if (auraEffectInfo.Mechanics != SpellMechanics.None && mechanicsFlags.HasTargetFlag(auraEffectInfo.Mechanics.AsFlag()))
                {
                    return true;
                }
            }

            return false;
        }

        public void CalculateDamageInterruptValue(Unit caster, Unit target, out int delay, out int interruptValue)
        {
            if (!interruptFlags.HasTargetFlag(AuraInterruptFlags.CombinedDamageTaken))
            {
                delay = 0;
                interruptValue = 0;
                return;
            }

            switch (interruptValueType)
            {
                case AuraInterruptValueCalculationType.Direct:
                    delay = damageInterruptDelay;
                    interruptValue = damageInterruptValue;
                    break;
                case AuraInterruptValueCalculationType.MaxHealthCasterPercent:
                    if (caster != null)
                    {
                        delay = damageInterruptDelay;
                        interruptValue = caster.MaxHealth.CalculatePercentage(damageInterruptValue);
                    }
                    else
                    {
                        delay = 0;
                        interruptValue = 0;
                    }
                    break;
                case AuraInterruptValueCalculationType.MaxHealthTargetPercent:
                    if (target != null)
                    {
                        delay = damageInterruptDelay;
                        interruptValue = target.MaxHealth.CalculatePercentage(damageInterruptValue);
                    }
                    else
                    {
                        delay = 0;
                        interruptValue = 0;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsStackableOnOneSlotWithDifferentCasters()
        {
            return maxStack > 1 && !HasAttribute(AuraAttributes.StackForAnyCasters);
        }
    }
}
