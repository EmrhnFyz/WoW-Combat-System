using Common;
using Core;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Animation Info", menuName = "Game Data/Animation/Spell Animation Info", order = 1)]
    public class SpellAnimationInfo : ScriptableUniqueInfo<SpellAnimationInfo>
    {
        [SerializeField] private SpellAnimationInfoContainer container;
        [SerializeField] private SpellInfo spellInfo;
        [SerializeField] private AnimationInfo animationInfo;

        protected override SpellAnimationInfo Data => this;
        protected override ScriptableUniqueInfoContainer<SpellAnimationInfo> Container => container;

        public SpellInfo Spell => spellInfo;
        public AnimationInfo Animation => animationInfo;
    }
}