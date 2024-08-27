using UnityEngine;

namespace Client.Actions
{
    [CreateAssetMenu(fileName = "Input Action - Toggle Option", menuName = "Player Data/Input/Actions/Toggle Option", order = 2)]
    public class ToggleOption : InputAction
    {
        [SerializeField] private GameOptionBool gameOption;

        public override void Execute()
        {
            gameOption.Toggle();
        }
    }
}
