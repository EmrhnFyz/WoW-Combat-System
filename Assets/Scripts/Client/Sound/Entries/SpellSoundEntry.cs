using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class SpellSoundEntry
    {
        public enum UsageType
        {
            Cast,
            Projectile,
            Impact,
            Aura,
            Destination
        }

        [SerializeField] private UsageType soundUsageType;
        [SerializeField] private SoundEntry soundEntry;

        public UsageType SoundUsageType => soundUsageType;

        public void PlayAtPoint(Vector3 point)
        {
            if (soundEntry != null)
            {
                soundEntry.PlayAtPoint(point);
            }
        }
    }
}