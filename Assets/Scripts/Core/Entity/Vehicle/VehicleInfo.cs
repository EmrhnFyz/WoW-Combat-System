using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Vehicle Info", menuName = "Game Data/Entities/Vehicle Info", order = 1)]
    public sealed class VehicleInfo : ScriptableUniqueInfo<VehicleInfo>
    {
        [SerializeField] private VehicleInfoContainer container;
        [SerializeField] private List<VehicleSeatInfo> vehicleSeats;

        protected override VehicleInfo Data => this;
        protected override ScriptableUniqueInfoContainer<VehicleInfo> Container => container;

        public new int Id => base.Id;
        public IReadOnlyList<VehicleSeatInfo> Seats => vehicleSeats;
    }
}
