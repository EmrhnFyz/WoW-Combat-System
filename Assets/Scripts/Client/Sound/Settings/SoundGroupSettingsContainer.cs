using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Sound Group Settings Container", menuName = "Game Data/Containers/Sound Group Settings", order = 1)]
    public class SoundGroupSettingsContainer : ScriptableUniqueInfoContainer<SoundGroupSettings>
    {
        [SerializeField] private SoundReference sound;
        [SerializeField] private List<SoundGroupSettings> groups;

        private readonly Dictionary<SoundGroupSettings, AudioSource> sourcesBySettings = new();

        protected override List<SoundGroupSettings> Items => groups;

        public override void Register()
        {
            base.Register();

            foreach (SoundGroupSettings soundSetting in groups)
            {
                sourcesBySettings[soundSetting] = ApplySettings(new GameObject(soundSetting.name).AddComponent<AudioSource>(), soundSetting);
            }
        }

        public override void Unregister()
        {
            foreach (KeyValuePair<SoundGroupSettings, AudioSource> soundSourceEntry in sourcesBySettings)
            {
                Destroy(soundSourceEntry.Value);
            }

            sourcesBySettings.Clear();

            base.Unregister();
        }

        public bool TryGetSource(SoundGroupSettings groupSettings, out AudioSource audioSource)
        {
            return sourcesBySettings.TryGetValue(groupSettings, out audioSource);
        }

        private AudioSource ApplySettings(AudioSource source, SoundGroupSettings settings)
        {
            source.transform.parent = sound.Container;
            return settings.Apply(source);
        }
    }
}