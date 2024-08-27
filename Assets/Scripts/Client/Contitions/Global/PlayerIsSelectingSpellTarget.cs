using Core.Conditions;
using UnityEngine;

namespace Client.Conditions
{
    [CreateAssetMenu(fileName = "Player Is Selecting Spell Target", menuName = "Game Data/Conditions/Player/Selecting Spell Target", order = 1)]
    public sealed class PlayerIsSelectingSpellTarget : Condition
    {
        [SerializeField] private TargetingSpellReference spellTargeting;

        protected override bool IsValid => base.IsValid && spellTargeting.IsTargeting;
    }
}
