using UnityEngine;

namespace Client.Actions
{
    [CreateAssetMenu(fileName = "Input Action - Select Target", menuName = "Player Data/Input/Actions/Select Target", order = 2)]
    public class SelectTarget : InputAction
    {
        [SerializeField] private TargetingReference targeting;
        [SerializeField] private TargetingOptions options;

        public override void Execute()
        {
            targeting.SelectTarget(options);
        }
    }
}
