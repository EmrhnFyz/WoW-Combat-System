using Core;
using UnityEngine;

namespace Client.Actions
{
    [CreateAssetMenu(fileName = "Input Action - Switch Class", menuName = "Player Data/Input/Actions/Switch Class", order = 2)]
    public class SwitchClass : InputAction
    {
        [SerializeField] private InputReference input;
        [SerializeField] private ClassType classType;

        public override void Execute() => input.SwitchClass(classType);
    }
}
