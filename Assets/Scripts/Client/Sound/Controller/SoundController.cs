using UnityEngine;

namespace Client
{
    public abstract class SoundController<TSoundKit, TKey> : MonoBehaviour where TSoundKit : SoundKit<TSoundKit, TKey>
    {
        [SerializeField] private SoundReference sound;
        [SerializeField] private AudioSource source;

        protected SoundReference Sound => sound;
        protected AudioSource Source => source;
        protected TSoundKit SoundKit { get; set; }

        protected TKey LastSoundType { get; private set; }

        public virtual void PlayOneShot(TKey soundType)
        {
            if (SoundKit == null)
            {
                return;
            }

            SoundEntry entry = SoundKit.FindSound(soundType, false);
            if (entry == null)
            {
                return;
            }

            LastSoundType = soundType;
            entry.PlayAtSource(source);
        }
    }
}
