using UnityEngine;

namespace Core.AuraEffects
{
    public abstract class AuraEffectInfoPeriodic : AuraEffectInfo
    {
        [SerializeField, Range(0, 30000)] private int period;
        [SerializeField,] private bool startPeriodicOnApply;

        public int Period => period;
        public bool StartPeriodicOnApply => startPeriodicOnApply;
    }
}