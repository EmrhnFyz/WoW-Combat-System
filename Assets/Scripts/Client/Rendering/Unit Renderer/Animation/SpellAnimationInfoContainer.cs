using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Animation Info Container", menuName = "Game Data/Containers/Spell Animation Info", order = 1)]
    public class SpellAnimationInfoContainer : ScriptableUniqueInfoContainer<SpellAnimationInfo>
    {
        [SerializeField] private SpellInfoContainer spellContainer;
        [SerializeField] private AnimationInfo defaultAnimation;
        [SerializeField] private List<SpellAnimationInfo> animationInfos;

        private readonly Dictionary<int, AnimationInfo> animationInfoBySpellId = new();

        protected override List<SpellAnimationInfo> Items => animationInfos;

        public override void Register()
        {
            base.Register();

            foreach (SpellAnimationInfo spellAnimation in animationInfos)
            {
                animationInfoBySpellId.Add(spellAnimation.Spell.Id, spellAnimation.Animation);
            }
        }

        public override void Unregister()
        {
            animationInfoBySpellId.Clear();

            base.Unregister();
        }

        public AnimationInfo FindAnimation(SpellInfo spellInfo)
        {
            if (animationInfoBySpellId.TryGetValue(spellInfo.Id, out AnimationInfo animationInfo))
            {
                return animationInfo;
            }

            return defaultAnimation;
        }
    }
}