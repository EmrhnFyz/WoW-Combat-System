using UnityEngine;

namespace Client
{
    public class AnimationRotationModifier : MonoBehaviour
    {
        [SerializeField] private Animator targetAnimator;
        [SerializeField] private Transform targetBone;
        [SerializeField] private Vector2 parameterRange;
        [SerializeField] private Vector3 maxRotation;
        [SerializeField] private string attackParam;
        [SerializeField] private string forwardParam;
        [SerializeField] private string strafeParam;
        [SerializeField] private bool overrideRotation;
        [SerializeField] private bool shouldApplyEvenBackward;
        [SerializeField] private bool shouldApplyOnlyWhenAttacking;
        [SerializeField] private bool revertWhenBackward;

        private int strafeHash;
        private int attackHash;
        private int forwardHash;

        private float attackValue;
        private float forwardValue;
        private float rotationValue = 0.5f;

        private void Awake()
        {
            strafeHash = Animator.StringToHash(strafeParam);
            attackHash = Animator.StringToHash(attackParam);
            forwardHash = Animator.StringToHash(forwardParam);
        }

        private void LateUpdate()
        {
            if (!targetAnimator.enabled)
            {
                return;
            }

            var currentValue = targetAnimator.GetFloat(strafeHash);
            var isAttacking = targetAnimator.GetBool(attackHash);
            forwardValue = targetAnimator.GetFloat(forwardHash);
            attackValue = Mathf.MoveTowards(attackValue, isAttacking ? 1.0f : 0.0f, 10 * Time.deltaTime);

            float newRotationValue;

            if (forwardValue < 0.0f && !shouldApplyEvenBackward)
            {
                newRotationValue = 0.5f;
            }
            else if (attackValue <= 0.5f && shouldApplyOnlyWhenAttacking)
            {
                newRotationValue = 0.5f;
            }
            else
            {
                newRotationValue = Mathf.InverseLerp(parameterRange.x, parameterRange.y, currentValue);
            }

            if (forwardValue < 0.0f && revertWhenBackward)
            {
                newRotationValue = 1.0f - newRotationValue;
            }

            rotationValue = Mathf.MoveTowards(rotationValue, newRotationValue, 2 * Time.deltaTime);

            Vector3 finalEulerRotation = (1 - (attackValue / 3)) * (1 - (Mathf.Abs(forwardValue) / 2)) * Vector3.Lerp(-maxRotation, maxRotation, rotationValue);
            targetBone.localRotation = overrideRotation ? Quaternion.Euler(finalEulerRotation)
                : targetBone.localRotation * Quaternion.Euler(finalEulerRotation);
        }
    }
}
