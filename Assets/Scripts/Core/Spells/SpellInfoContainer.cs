using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Spell Info Container", menuName = "Game Data/Containers/Spell Info", order = 1)]
    public class SpellInfoContainer : ScriptableUniqueInfoContainer<SpellInfo>
    {
        [SerializeField] private List<SpellInfo> spellInfo;

        [Header("Unique Spells")]
        [SerializeField] private SpellInfo controlVehicle;

        protected override List<SpellInfo> Items => spellInfo;

        public SpellInfo ControlVehicle => controlVehicle;
    }
}
