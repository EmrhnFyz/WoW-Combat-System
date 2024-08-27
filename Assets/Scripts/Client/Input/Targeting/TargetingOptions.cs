using Common;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TargetingOptions
    {
        [SerializeField, EnumFlag] private TargetingDeathState deathState;
        [SerializeField, EnumFlag] private TargetingEntityType entityTypes;
        [SerializeField] private TargetingDistance distance;
        [SerializeField] private TargetingMode mode;
        [SerializeField] private float maxReferringAngle = 180.0f;

        public TargetingDeathState DeathState => deathState;
        public TargetingEntityType EntityTypes => entityTypes;
        public TargetingDistance Distance => distance;
        public TargetingMode Mode => mode;
        public float MaxReferringAngle => maxReferringAngle;
    }
}
