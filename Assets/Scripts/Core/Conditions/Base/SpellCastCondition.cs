using UnityEngine;

namespace Core.Conditions
{
    [CreateAssetMenu(fileName = "Spell Cast Condition", menuName = "Game Data/Conditions/Base/Spell Cast Condition", order = 1)]
    public sealed class SpellCastCondition : Condition
    {
        [SerializeField] private SpellCastResult failedResult;
        [SerializeField] private Condition condition;

        public SpellCastResult FailedResult => failedResult;

        protected override bool IsApplicable => base.IsApplicable && IsOtherApplicable(condition);

        protected override bool IsValid => base.IsValid && IsOtherValid(condition);
    }
}
