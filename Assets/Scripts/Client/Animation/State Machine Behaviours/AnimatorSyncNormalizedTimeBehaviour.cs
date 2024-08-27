using UnityEngine;

namespace Client
{
    public class AnimatorSyncNormalizedTimeBehaviour : StateMachineBehaviour
    {
        [SerializeField] private int otherLayerIndex;

        private bool alreadySynced;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (alreadySynced)
            {
                alreadySynced = false;
            }
            else
            {
                AnimatorStateInfo otherStateInfo = animator.GetCurrentAnimatorStateInfo(otherLayerIndex);

                if (otherStateInfo.shortNameHash == animatorStateInfo.shortNameHash)
                {
                    alreadySynced = true;
                    animator.CrossFade(animatorStateInfo.shortNameHash, 0.2f, layerIndex, otherStateInfo.normalizedTime);
                }
            }
        }
    }
}
