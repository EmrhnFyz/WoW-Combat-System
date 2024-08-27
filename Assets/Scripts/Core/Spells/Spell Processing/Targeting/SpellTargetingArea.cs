using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Area Target - Spell Targeting", menuName = "Game Data/Spells/Spell Targeting/Area", order = 1)]
    public class SpellTargetingArea : SpellTargeting
    {
        [SerializeField] private SpellTargetReferences referenceType = SpellTargetReferences.Caster;
        [SerializeField] private SpellTargetChecks targetChecks = SpellTargetChecks.Enemy;
        [SerializeField] private float minRadius;
        [SerializeField] private float maxRadius = 10.0f;

        public float MaxRadius => maxRadius;

        private Vector3 SelectSource(SpellExplicitTargets explicitTargets, Unit caster)
        {
            return referenceType switch
            {
                SpellTargetReferences.Destination => explicitTargets.Destination ?? caster.Position,
                SpellTargetReferences.Source or SpellTargetReferences.Caster => caster.Position,
                SpellTargetReferences.Target => explicitTargets.Target.Position,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private float CalculateRadius(Spell spell)
        {
            var radius = Mathf.Max(maxRadius, minRadius);
            if (spell.Caster != null)
            {
                radius = spell.Caster.Spells.ApplySpellModifier(spell, SpellModifierType.Radius, radius);
                radius = Mathf.Clamp(radius, minRadius, maxRadius);
            }

            return radius;
        }

        protected virtual bool IsValidTargetForSpell(Unit target, Spell spell)
        {
            if (target.IsDead && !spell.SpellInfo.HasAttribute(SpellAttributes.CanTargetDead))
            {
                return false;
            }

            return true;
        }

        internal sealed override void SelectTargets(Spell spell, int effectMask)
        {
            Vector3 center = SelectSource(spell.ExplicitTargets, spell.Caster);
            var radius = CalculateRadius(spell);
            var targets = new List<Unit>();

            spell.Caster.Map.SearchAreaTargets(targets, radius, center, spell.Caster, targetChecks);

            foreach (Unit target in targets)
            {
                if (IsValidTargetForSpell(target, spell))
                {
                    spell.ImplicitTargets.AddTargetIfNotExists(target, effectMask);
                }
            }
        }
    }
}
