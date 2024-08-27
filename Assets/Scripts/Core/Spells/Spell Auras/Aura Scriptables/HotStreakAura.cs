using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Hot Streak Aura Script", menuName = "Game Data/Spells/Auras/Aura Scripts/Hot Streak", order = 1)]
    internal class HotStreakAura : AuraScriptable, IAuraScriptSpellDamageHandler
    {
        [SerializeField] private List<SpellInfo> critCheckedSpells;
        [SerializeField] private SpellInfo heatingUpSpell;
        [SerializeField] private SpellInfo hotStreakSpell;

        public void OnSpellDamageDone(SpellDamageInfo damageInfo)
        {
            if (!critCheckedSpells.Contains(damageInfo.SpellInfo) || damageInfo.SpellDamageType != SpellDamageType.Direct)
            {
                return;
            }

            var isCrit = damageInfo.HitType.HasTargetFlag(HitType.CriticalHit);
            if (isCrit)
            {
                if (damageInfo.Caster.Auras.HasAuraWithSpell(heatingUpSpell.Id))
                {
                    damageInfo.Caster.Auras.RemoveAuraWithSpellInfo(heatingUpSpell, AuraRemoveMode.Spell);
                    damageInfo.Caster.Spells.TriggerSpell(hotStreakSpell, damageInfo.Caster);
                }
                else
                {
                    damageInfo.Caster.Spells.TriggerSpell(heatingUpSpell, damageInfo.Caster);
                }
            }
            else
            {
                damageInfo.Caster.Auras.RemoveAuraWithSpellInfo(heatingUpSpell, AuraRemoveMode.Spell);
            }
        }
    }
}