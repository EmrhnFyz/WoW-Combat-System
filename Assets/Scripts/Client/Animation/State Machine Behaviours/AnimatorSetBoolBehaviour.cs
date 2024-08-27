using UnityEngine;

namespace Client
{
    public class AnimatorSetBoolBehaviour : StateMachineBehaviour
    {
        [SerializeField] private string parameterName;
        [SerializeField] private bool valueOnEnter;
        [SerializeField] private bool valueOnExit;
        [SerializeField] private bool valueOnMachineEnter;
        [SerializeField] private bool valueOnMachineExit;
        [SerializeField] private bool setOnEnter = true;
        [SerializeField] private bool setOnExit = true;
        [SerializeField] private bool setOnMachineEnter;
        [SerializeField] private bool setOnMachineExit;

        private int parameterHash;

        private void OnEnable()
        {
            parameterHash = Animator.StringToHash(parameterName);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (setOnEnter)
            {
                animator.SetBool(parameterHash, valueOnEnter);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (setOnExit)
            {
                if (animator.GetCurrentAnimatorStateInfo(layerIndex).shortNameHash != animatorStateInfo.shortNameHash)
                {
                    animator.SetBool(parameterHash, valueOnExit);
                }
            }
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            if (setOnMachineEnter)
            {
                animator.SetBool(parameterHash, valueOnMachineEnter);
            }
        }

        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            if (setOnMachineExit)
            {
                animator.SetBool(parameterHash, valueOnMachineExit);
            }
        }
    }
}
