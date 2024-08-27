using Common;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Sound Group Settings", menuName = "Game Data/Sound/Sound Group Settings", order = 1)]
    public class SoundGroupSettings : ScriptableUniqueInfo<SoundGroupSettings>
    {
        [SerializeField] private SoundGroupSettingsContainer container;
        [SerializeField] private float volume;
        [SerializeField] private float spatialBlend;

        protected override SoundGroupSettings Data => this;
        protected override ScriptableUniqueInfoContainer<SoundGroupSettings> Container => container;

        public AudioSource Apply(AudioSource source)
        {
            source.volume = volume;
            source.spatialBlend = spatialBlend;
            return source;
        }
    }
}
