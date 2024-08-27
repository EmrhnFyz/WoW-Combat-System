using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Targeting Settings", menuName = "Player Data/Input/Targeting Settings", order = 1)]
    public class TargetingSettings : ScriptableObject
    {
        [SerializeField] private float targetRange;
        [SerializeField, HideInInspector] private float targetRangeSqr;

        public float TargetRange => targetRange;
        public float TargetRangeSqr => targetRangeSqr;


        private void OnValidate()
        {
            targetRangeSqr = targetRange * targetRange;
        }
    }
}
