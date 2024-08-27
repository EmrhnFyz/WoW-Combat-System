using Client.Spells;
using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Visual Info Container", menuName = "Game Data/Containers/Spell Visual Info", order = 1)]
    public class SpellVisualsInfoContainer : ScriptableUniqueInfoContainer<SpellVisualsInfo>
    {
        [SerializeField] private List<SpellVisualsInfo> visualsInfos;

        protected override List<SpellVisualsInfo> Items => visualsInfos;

        private readonly Dictionary<int, SpellVisualsInfo> spellVisualsInfosById = new();

        public IReadOnlyDictionary<int, SpellVisualsInfo> SpellVisualsInfosById => spellVisualsInfosById;

        public override void Register()
        {
            base.Register();

            visualsInfos.ForEach(visual => spellVisualsInfosById.Add(visual.SpellInfo.Id, visual));
            visualsInfos.ForEach(visual => visual.Initialize());
        }

        public override void Unregister()
        {
            visualsInfos.ForEach(visual => visual.Deinitialize());
            spellVisualsInfosById.Clear();

            base.Unregister();
        }
    }
}