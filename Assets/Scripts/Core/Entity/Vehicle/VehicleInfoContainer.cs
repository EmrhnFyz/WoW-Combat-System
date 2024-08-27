using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Vehicle Info Container", menuName = "Game Data/Containers/Vehicle Info", order = 1)]
    internal class VehicleInfoContainer : ScriptableUniqueInfoContainer<VehicleInfo>
    {
        [SerializeField] private List<VehicleInfo> vehicleInfos;

        protected override List<VehicleInfo> Items => vehicleInfos;

        private readonly Dictionary<int, VehicleInfo> vehicleInfoById = new();

        public IReadOnlyDictionary<int, VehicleInfo> VehicleInfoById => vehicleInfoById;

        public override void Register()
        {
            base.Register();

            vehicleInfos.ForEach(vehicle => vehicleInfoById.Add(vehicle.Id, vehicle));
        }

        public override void Unregister()
        {
            vehicleInfoById.Clear();

            base.Unregister();
        }
    }
}