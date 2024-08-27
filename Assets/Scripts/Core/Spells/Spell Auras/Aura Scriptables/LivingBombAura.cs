﻿using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Living Bomb Aura Script", menuName = "Game Data/Spells/Auras/Aura Scripts/Living Bomb", order = 1)]
    internal class LivingBombAura : AuraScriptable
    {
        [SerializeField] private SpellInfo livingBombExplosion;

        public override void AuraApplicationRemoved(AuraApplication application)
        {
            base.AuraApplicationRemoved(application);

            if (application.Aura.Caster == null)
            {
                return;
            }

            if (application.RemoveMode is AuraRemoveMode.Death or AuraRemoveMode.Expired)
            {
                var explicitTargets = new SpellExplicitTargets { Target = application.Aura.Owner };
                var castingOptions = new SpellCastingOptions(explicitTargets, SpellCastFlags.TriggeredByAura);
                application.Aura.Caster.Spells.CastSpell(livingBombExplosion, castingOptions);
            }
        }
    }
}