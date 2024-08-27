using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Unit AI - State Machine", menuName = "Game Data/AI/State Machine", order = 1)]
    public sealed class UnitInfoAIStateMachine : UnitInfoAISettings
    {
        [SerializeField] private Animator prototype;

        public Animator Prototype => prototype;

        public override IUnitAIModel CreateAI() => new UnitStateMachine(this);
    }
}
