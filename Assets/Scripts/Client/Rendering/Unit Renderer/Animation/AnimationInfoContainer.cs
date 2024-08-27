using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Animation Info Container", menuName = "Game Data/Containers/Animation Info", order = 1)]
    public class AnimationInfoContainer : ScriptableUniqueInfoContainer<AnimationInfo>
    {
        [SerializeField] private List<AnimationInfo> modelSettings;

        protected override List<AnimationInfo> Items => modelSettings;
    }
}