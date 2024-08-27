using Common;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Vehicle Seat Info", menuName = "Game Data/Entities/Vehicle Seat Info", order = 1)]
    public sealed class VehicleSeatInfo : ScriptableUniqueInfo<VehicleSeatInfo>
    {
        [SerializeField] private VehicleSeatInfoContainer container;
        [SerializeField, EnumFlag] private VehicleSeatFlags flags;

        protected override VehicleSeatInfo Data => this;
        protected override ScriptableUniqueInfoContainer<VehicleSeatInfo> Container => container;

        public new int Id => base.Id;
        public VehicleSeatFlags Flags => flags;
    }
}