using Core;
using Core.AuraEffects;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class SpellTooltipArgumentSettings
    {
        [SerializeField] private ScriptableObject argumentSource;
        [SerializeField] private SpellTooltipArgumentType argumentType;

        public float? Resolve()
        {
            float? result;
            switch (argumentSource)
            {
                case SpellInfo spellInfo:
                    result = Resolve(spellInfo);
                    break;
                case SpellEffectInfo spellEffectInfo:
                    result = Resolve(spellEffectInfo);
                    break;
                case AuraInfo auraInfo:
                    result = Resolve(auraInfo);
                    break;
                case AuraEffectInfo auraEffectInfo:
                    result = Resolve(auraEffectInfo);
                    break;
                default:
                    return null;
            }

            return result;
        }

        private float? Resolve(SpellInfo spellInfo)
        {
            return argumentType switch
            {
                _ => null,
            };
        }

        private float? Resolve(SpellEffectInfo spellEffectInfo)
        {
            return argumentType switch
            {
                SpellTooltipArgumentType.Value => spellEffectInfo.Value,
                SpellTooltipArgumentType.Radius when spellEffectInfo.Targeting is SpellTargetingArea areaTargeting => areaTargeting.MaxRadius,
                _ => null,
            };
        }

        private float? Resolve(AuraInfo auraInfo)
        {
            return argumentType switch
            {
                SpellTooltipArgumentType.Duration => (float)auraInfo.MaxDuration / 1000,
                _ => null,
            };
        }

        private float? Resolve(AuraEffectInfo auraEffectInfo)
        {
            switch (argumentType)
            {
                case SpellTooltipArgumentType.Period when auraEffectInfo is AuraEffectInfoPeriodic periodicEffect:
                    return (float)periodicEffect.Period / 1000;
                case SpellTooltipArgumentType.Value when auraEffectInfo is AuraEffectInfoSpellModifier:
                case SpellTooltipArgumentType.Value when auraEffectInfo is AuraEffectInfoModifyDamagePercentTaken:
                    return Mathf.Abs(auraEffectInfo.Value);
                case SpellTooltipArgumentType.Value:
                    return auraEffectInfo.Value;
                default:
                    return null;
            }
        }

#if UNITY_EDITOR
        public bool Validate()
        {
            return argumentSource switch
            {
                SpellInfo spellInfo => Resolve(spellInfo) != null,
                SpellEffectInfo spellEffectInfo => Resolve(spellEffectInfo) != null,
                AuraInfo auraInfo => Resolve(auraInfo) != null,
                AuraEffectInfo auraEffectInfo => Resolve(auraEffectInfo) != null,
                _ => false,
            };
        }
#endif
    }
}
