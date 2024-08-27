using UnityEngine;

namespace Client.Actions
{
    [CreateAssetMenu(fileName = "Input Action - Stop Spell Targeting", menuName = "Player Data/Input/Actions/Stop Spell Targeting", order = 1)]
    public class StopSpellTargeting : InputAction
    {
        [SerializeField] private TargetingSpellReference spellTargeting;

        public override void Execute()
        {
            spellTargeting.StopTargeting();
        }
    }
}