using UnityEngine;

namespace Core.Conditions
{
    [CreateAssetMenu(fileName = "Inverse Condition", menuName = "Game Data/Conditions/Base/Inverse Condition", order = 1)]
    public sealed class InverseCondition : Condition
    {
        [SerializeField] private Condition condition;

        protected override bool IsApplicable => base.IsApplicable && IsOtherApplicable(condition);

        protected override bool IsValid => base.IsValid && !IsOtherValid(condition);
    }
}
