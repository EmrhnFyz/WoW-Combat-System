using Common;
using UnityEngine;

namespace Core.Conditions
{
    [CreateAssetMenu(fileName = "Spell With Base Cast Time", menuName = "Game Data/Conditions/Spell/Spell With Base Cast Time", order = 2)]
    public sealed class SpellWithBaseCastTime : Condition
    {
        [SerializeField] private ComparisonOperator comparisonOperator;
        [SerializeField] private int castTime;

        protected override bool IsApplicable => base.IsApplicable && SpellInfo != null;

        protected override bool IsValid => base.IsValid && SpellInfo.CastTime.CompareWith(castTime, comparisonOperator);
    }
}