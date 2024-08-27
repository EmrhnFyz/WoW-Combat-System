using System;

namespace Core
{
    public static class SpellUtils
    {
        public static bool HasTargetFlag(this SpellInterruptFlags baseFlags, SpellInterruptFlags flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasTargetFlag(this AuraInterruptFlags baseFlags, AuraInterruptFlags flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasAnyFlag(this AuraInterruptFlags baseFlags, AuraInterruptFlags flag)
        {
            return (baseFlags & flag) != 0;
        }

        public static bool HasTargetFlag(this SpellSchoolMask baseFlags, SpellSchoolMask flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasAnyFlag(this SpellSchoolMask baseFlags, SpellSchoolMask flag)
        {
            return (baseFlags & flag) != 0;
        }

        public static bool HasAnyFlag(this SpellTriggerFlags baseFlags, SpellTriggerFlags flag)
        {
            return (baseFlags & flag) != 0;
        }

        public static bool HasTargetFlag(this HitType baseFlags, HitType flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasTargetFlag(this SpellMechanicsFlags baseFlags, SpellMechanicsFlags flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasAnyFlag(this SpellMechanicsFlags baseFlags, SpellMechanicsFlags flag)
        {
            return (baseFlags & flag) != 0;
        }

        public static bool HasTargetFlag(this UnitVisualEffectFlags baseFlags, UnitVisualEffectFlags flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasAnyFlag(this UnitVisualEffectFlags baseFlags, UnitVisualEffectFlags flag)
        {
            return (baseFlags & flag) != 0;
        }

        public static UnitVisualEffectFlags SetFlag(this UnitVisualEffectFlags baseFlags, UnitVisualEffectFlags flag, bool set)
        {
            return set ? baseFlags | flag : baseFlags & ~flag;
        }

        public static bool HasTargetFlag(this SpellPreventionType baseFlags, SpellPreventionType flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasAnyFlag(this SpellPreventionType baseFlags, SpellPreventionType flag)
        {
            return (baseFlags & flag) != 0;
        }

        public static bool HasTargetFlag(this SpellCastFlags baseFlags, SpellCastFlags flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasTargetFlag(this SpellRangeFlags baseFlags, SpellRangeFlags flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasTargetFlag(this SpellCastTargetFlags baseFlags, SpellCastTargetFlags flag)
        {
            return (baseFlags & flag) == flag;
        }

        public static bool HasAnyFlag(this SpellCastTargetFlags baseFlags, SpellCastTargetFlags flag)
        {
            return (baseFlags & flag) != 0;
        }

        public static AuraStateFlags AsFlag(this AuraStateType auraStateType)
        {
            return auraStateType switch
            {
                AuraStateType.None => 0,
                AuraStateType.Frozen => AuraStateFlags.Frozen,
                AuraStateType.Defense => AuraStateFlags.Defense,
                AuraStateType.Berserking => AuraStateFlags.Berserking,
                AuraStateType.Judgement => AuraStateFlags.Judgement,
                AuraStateType.Conflagrate => AuraStateFlags.Conflagrate,
                AuraStateType.Swiftmend => AuraStateFlags.Swiftmend,
                AuraStateType.DeadlyPoison => AuraStateFlags.DeadlyPoison,
                AuraStateType.Enrage => AuraStateFlags.Enrage,
                AuraStateType.Bleeding => AuraStateFlags.Bleeding,
                _ => throw new ArgumentOutOfRangeException(nameof(auraStateType), auraStateType, null),
            };
        }

        public static SpellMechanicsFlags AsFlag(this SpellMechanics mechanics)
        {
            return mechanics switch
            {
                SpellMechanics.None => 0,
                SpellMechanics.Charm => SpellMechanicsFlags.Charm,
                SpellMechanics.Disoriented => SpellMechanicsFlags.Disoriented,
                SpellMechanics.Disarm => SpellMechanicsFlags.Disarm,
                SpellMechanics.Distract => SpellMechanicsFlags.Distract,
                SpellMechanics.Fear => SpellMechanicsFlags.Fear,
                SpellMechanics.Grip => SpellMechanicsFlags.Grip,
                SpellMechanics.Root => SpellMechanicsFlags.Root,
                SpellMechanics.SlowAttack => SpellMechanicsFlags.SlowAttack,
                SpellMechanics.Silence => SpellMechanicsFlags.Silence,
                SpellMechanics.Sleep => SpellMechanicsFlags.Sleep,
                SpellMechanics.Snare => SpellMechanicsFlags.Snare,
                SpellMechanics.Stun => SpellMechanicsFlags.Stun,
                SpellMechanics.Freeze => SpellMechanicsFlags.Freeze,
                SpellMechanics.Knockout => SpellMechanicsFlags.Knockout,
                SpellMechanics.Bleed => SpellMechanicsFlags.Bleed,
                SpellMechanics.Bandage => SpellMechanicsFlags.Bandage,
                SpellMechanics.Polymorph => SpellMechanicsFlags.Polymorph,
                SpellMechanics.Banish => SpellMechanicsFlags.Banish,
                SpellMechanics.Shield => SpellMechanicsFlags.Shield,
                SpellMechanics.Shackle => SpellMechanicsFlags.Shackle,
                SpellMechanics.Mount => SpellMechanicsFlags.Mount,
                SpellMechanics.Infected => SpellMechanicsFlags.Infected,
                SpellMechanics.Horror => SpellMechanicsFlags.Horror,
                SpellMechanics.Invulnerability => SpellMechanicsFlags.Invulnerability,
                SpellMechanics.Interrupt => SpellMechanicsFlags.Interrupt,
                SpellMechanics.Daze => SpellMechanicsFlags.Daze,
                SpellMechanics.ImmuneShield => SpellMechanicsFlags.ImmuneShield,
                SpellMechanics.Sapped => SpellMechanicsFlags.Sapped,
                SpellMechanics.Enraged => SpellMechanicsFlags.Enraged,
                SpellMechanics.Wounded => SpellMechanicsFlags.Wounded,
                _ => throw new ArgumentOutOfRangeException(nameof(mechanics), mechanics, null),
            };
        }
    }
}