using Common;
using Core;
using UnityEngine;

namespace Client.Spells
{
    [CreateAssetMenu(fileName = "Aura Visual Info", menuName = "Game Data/Visuals/Aura Visual Info", order = 3)]
    public class AuraVisualsInfo : ScriptableUniqueInfo<AuraVisualsInfo>, IEffectPositionerSettings
    {
        [SerializeField] private AuraVisualsInfoContainer container;
        [SerializeField] private AuraInfo auraInfo;
        [SerializeField] private Sprite auraIcon;
        [SerializeField] private EffectTagType tagType;
        [SerializeField] private EffectSettings effectSettings;
        [SerializeField] private bool keepOriginalRotation = true;
        [SerializeField] private bool preventAnimation;

        protected override AuraVisualsInfo Data => this;
        protected override ScriptableUniqueInfoContainer<AuraVisualsInfo> Container => container;

        public AuraInfo AuraInfo => auraInfo;
        public Sprite AuraIcon => auraIcon;
        public EffectSettings EffectSettings => effectSettings;
        public EffectTagType EffectTagType => tagType;
        public bool AttachToTag => true;
        public bool KeepOriginalRotation => keepOriginalRotation;
        public bool PreventAnimation => preventAnimation;
        public bool KeepAliveWithNoParticles => true;
    }
}