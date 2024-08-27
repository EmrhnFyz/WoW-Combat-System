using Common;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Creature Info", menuName = "Game Data/Entities/Creature Info", order = 2)]
    internal sealed class CreatureInfo : ScriptableUniqueInfo<CreatureInfo>
    {
        [SerializeField] private CreatureInfoContainer container;
        [SerializeField] private VehicleInfo vehicleInfo;
        [SerializeField] private string creatureName;
        [SerializeField] private int modelId;

        protected override ScriptableUniqueInfoContainer<CreatureInfo> Container => container;
        protected override CreatureInfo Data => this;

        public new int Id => base.Id;
        public int ModelId => modelId;
        public string CreatureName => creatureName;
        public VehicleInfo VehicleInfo => vehicleInfo;
    }
}
