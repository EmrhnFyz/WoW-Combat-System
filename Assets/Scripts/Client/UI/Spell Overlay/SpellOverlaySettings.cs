using Common;
using Core;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Overlay Settings", menuName = "Game Data/Interface/Spell Overlay Settings", order = 2)]
    public class SpellOverlaySettings : ScriptableUniqueInfo<SpellOverlaySettings>
    {
        [SerializeField] private SpellOverlaySettingsContainer container;
        [SerializeField] private SpellOverlay prototype;
        [SerializeField] private AuraInfo triggerAura;

        protected override ScriptableUniqueInfoContainer<SpellOverlaySettings> Container => container;
        protected override SpellOverlaySettings Data => this;

        public AuraInfo TriggerAura => triggerAura;
        public SpellOverlay Prototype => prototype;
    }
}
