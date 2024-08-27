using System;
using UnityEngine;

namespace Client.Spells
{
    [Serializable]
    public class EffectSpellSettings : IEffectPositionerSettings
    {
        public enum UsageType
        {
            Cast,
            Projectile,
            Impact,
            Destination
        }

        [SerializeField] private bool attachToTag;
        [SerializeField] private UsageType visualUsageType;
        [SerializeField] private EffectTagType tagType;
        [SerializeField] private EffectSettings effectSettings;

        public UsageType VisualUsageType => visualUsageType;
        public EffectTagType EffectTagType => tagType;
        public EffectSettings EffectSettings => effectSettings;
        public bool AttachToTag => attachToTag;
        public bool KeepOriginalRotation => false;
        public bool KeepAliveWithNoParticles => visualUsageType == UsageType.Projectile;
    }
}
