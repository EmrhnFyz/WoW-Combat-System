using UnityEngine;

namespace Core.Conditions
{
    [CreateAssetMenu(fileName = "Target Unit Has Aura State", menuName = "Game Data/Conditions/Unit/Target Has Aura State", order = 3)]
    public sealed class TargetUnitHasAuraState : Condition
    {
        [SerializeField] private AuraStateType auraStateType;

        protected override bool IsApplicable => base.IsApplicable && TargetUnit != null;

        protected override bool IsValid => base.IsValid && TargetUnit.HasAuraState(auraStateType, SourceUnit, Spell);
    }
}