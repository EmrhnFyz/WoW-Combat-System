using UnityEngine;

namespace Client
{
    public class AnimatorSetFloatBehaviour : StateMachineBehaviour
    {
        [SerializeField] private string parameterName;
        [SerializeField] private float valueOnEnter;
        [SerializeField] private float valueOnExit;
        [SerializeField] private bool setOnEnter = true;
        [SerializeField] private bool setOnExit = true;

        private int parameterHash;

        private void OnEnable()
        {
            parameterHash = Animator.StringToHash(parameterName);
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (setOnEnter)
            {
                animator.SetFloat(parameterHash, valueOnEnter);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (setOnExit)
            {
                if (animator.GetCurrentAnimatorStateInfo(layerIndex).shortNameHash != animatorStateInfo.shortNameHash)
                {
                    animator.SetFloat(parameterHash, valueOnExit);
                }
            }
        }
    }
}
