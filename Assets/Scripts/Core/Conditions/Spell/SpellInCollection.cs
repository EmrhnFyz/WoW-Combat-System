using System.Collections.Generic;
using UnityEngine;

namespace Core.Conditions
{
    [CreateAssetMenu(fileName = "Spell In Collection", menuName = "Game Data/Conditions/Spell/Spell In Collection", order = 2)]
    public sealed class SpellInCollection : Condition
    {
        [SerializeField] private List<SpellInfo> validSpells;

        protected override bool IsApplicable => base.IsApplicable && SpellInfo != null;

        protected override bool IsValid => base.IsValid && validSpells.Contains(SpellInfo);
    }
}