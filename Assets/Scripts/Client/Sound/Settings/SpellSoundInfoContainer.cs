using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Sound Info Container", menuName = "Game Data/Containers/Spell Sound Info", order = 1)]
    public class SpellSoundInfoContainer : ScriptableUniqueInfoContainer<SpellSoundInfo>
    {
        [SerializeField] private List<SpellSoundInfo> soundInfos;

        protected override List<SpellSoundInfo> Items => soundInfos;

        private readonly Dictionary<SpellInfo, SpellSoundInfo> spellSoundInfos = new();

        public IReadOnlyDictionary<SpellInfo, SpellSoundInfo> SoundInfos => spellSoundInfos;

        public override void Register()
        {
            base.Register();

            foreach (SpellSoundInfo spellSetting in soundInfos)
            {
                spellSoundInfos[spellSetting.SpellInfo] = spellSetting;
            }
        }

        public override void Unregister()
        {
            spellSoundInfos.Clear();

            base.Unregister();
        }
    }
}