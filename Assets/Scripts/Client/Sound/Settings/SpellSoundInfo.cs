using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Sound Info", menuName = "Game Data/Sound/Spell Sound Info", order = 1)]
    public class SpellSoundInfo : ScriptableUniqueInfo<SpellSoundInfo>
    {
        [SerializeField] private SpellSoundInfoContainer container;
        [SerializeField] private SpellInfo spellInfo;
        [SerializeField] private List<SpellSoundEntry> soundEntries = new();

        protected override SpellSoundInfo Data => this;
        protected override ScriptableUniqueInfoContainer<SpellSoundInfo> Container => container;

        public SpellInfo SpellInfo => spellInfo;

        public void PlayAtPoint(Vector3 point, SpellSoundEntry.UsageType usageType)
        {
            soundEntries.Find(entry => entry.SoundUsageType == usageType)?.PlayAtPoint(point);
        }
    }
}