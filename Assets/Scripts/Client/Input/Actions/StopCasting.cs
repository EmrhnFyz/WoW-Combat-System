using UnityEngine;

namespace Client.Actions
{
    [CreateAssetMenu(fileName = "Input Action - Stop Casting", menuName = "Player Data/Input/Actions/Stop Casting", order = 1)]
    public class StopCasting : InputAction
    {
        [SerializeField] private InputReference inputReference;

        public override void Execute()
        {
            inputReference.StopCasting();
        }
    }
}
