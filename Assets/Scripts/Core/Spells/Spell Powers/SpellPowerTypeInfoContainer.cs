using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Spell Power Type Info Container", menuName = "Game Data/Containers/Spell Power Type Info", order = 1)]
    public class SpellPowerTypeInfoContainer : ScriptableUniqueInfoContainer<SpellPowerTypeInfo>
    {
        [SerializeField] private List<SpellPowerTypeInfo> spellPowerTypeInfos;

        protected override List<SpellPowerTypeInfo> Items => spellPowerTypeInfos;
    }
}
