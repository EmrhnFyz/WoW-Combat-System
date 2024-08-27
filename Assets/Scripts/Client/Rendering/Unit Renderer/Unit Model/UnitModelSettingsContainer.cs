using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Unit Model Settings Container", menuName = "Game Data/Containers/Unit Model Settings", order = 1)]
    public class UnitModelSettingsContainer : ScriptableUniqueInfoContainer<UnitModelSettings>
    {
        [SerializeField] private List<UnitModelSettings> modelSettings;

        private readonly Dictionary<int, UnitModelSettings> modelSettingsById = new();

        protected override List<UnitModelSettings> Items => modelSettings;

        public IReadOnlyDictionary<int, UnitModelSettings> ModelSettingsById => modelSettingsById;

        public override void Register()
        {
            base.Register();

            for (var i = 0; i < ItemList.Count; i++)
            {
                modelSettingsById.Add(ItemList[i].Id, ItemList[i]);
            }
        }

        public override void Unregister()
        {
            modelSettingsById.Clear();

            base.Unregister();
        }
    }
}
