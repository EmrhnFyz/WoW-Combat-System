using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Unit Info AI Container", menuName = "Game Data/Containers/Unit Info AI", order = 1)]
    public class UnitInfoAIContainer : ScriptableUniqueInfoContainer<UnitInfoAI>
    {
        [SerializeField] private List<UnitInfoAI> aiInfos;

        protected override List<UnitInfoAI> Items => aiInfos;
    }
}
