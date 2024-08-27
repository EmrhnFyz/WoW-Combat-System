using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Overlay Settings Container", menuName = "Game Data/Containers/Spell Overlay Settings", order = 2)]
    public class SpellOverlaySettingsContainer : ScriptableUniqueInfoContainer<SpellOverlaySettings>
    {
        [SerializeField] private List<SpellOverlaySettings> settings;

        protected override List<SpellOverlaySettings> Items => settings;
    }
}
