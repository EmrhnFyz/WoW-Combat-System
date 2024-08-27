using Common;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Unit Model Settings", menuName = "Player Data/Rendering/Unit Model", order = 3)]
    public sealed class UnitModelSettings : ScriptableUniqueInfo<UnitModelSettings>
    {
        [SerializeField] private UnitModelSettingsContainer container;
        [SerializeField] private UnitModel prototype;
        [SerializeField] private UnitSoundKit soundKit;

        protected override ScriptableUniqueInfoContainer<UnitModelSettings> Container => container;
        protected override UnitModelSettings Data => this;

        public new int Id => base.Id;
        public UnitSoundKit SoundKit => soundKit;
        public UnitModel Prototype => prototype;
    }
}
