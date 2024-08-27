using Common;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Unit Sound Kit", menuName = "Game Data/Sound/Unit Sound Kit", order = 1)]
    public sealed class UnitSoundKit : SoundKit<UnitSoundKit, UnitSounds>
    {
        [SerializeField] private UnitSoundKitContainer continer;
        [SerializeField] private SoundEntryCharacterSoundDictionary soundItems;

        protected override ScriptableUniqueInfoContainer<UnitSoundKit> Container => continer;
        protected override UnitSoundKit Data => this;

        public new int Id => base.Id;

        protected override void OnRegister() => soundItems.Register();

        protected override void OnUnregister() => soundItems.Unregister();

        public override SoundEntry FindSound(UnitSounds soundType, bool allowDefault)
        {
            if (!soundItems.ValuesByKey.TryGetValue(soundType, out SoundEntry result))
            {
                return !allowDefault ? null : soundItems.DefaultValue;
            }

            return result;
        }
    }
}
