using UnityEngine;

namespace Core.Conditions
{
    [CreateAssetMenu(fileName = "Source Unit Is Casting", menuName = "Game Data/Conditions/Unit/Source Unit Is Casting", order = 1)]
    public sealed class SourceUnitIsCasting : Condition
    {
        protected override bool IsApplicable => base.IsApplicable && SourceUnit != null;

        protected override bool IsValid => base.IsValid && SourceUnit.SpellCast.IsCasting;
    }
}
