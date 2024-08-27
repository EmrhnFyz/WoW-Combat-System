using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TagContainer : IEffectPositioner
    {
        [SerializeField] private EffectTagType defaultLaunchTag = EffectTagType.LeftHand;
        [SerializeField] private Transform defaultTag;
        [SerializeField] private Transform bottomTag;
        [SerializeField] private Transform footTag;
        [SerializeField] private Transform impactTag;
        [SerializeField] private Transform impactStaticTag;
        [SerializeField] private Transform rightHandTag;
        [SerializeField] private Transform leftHandTag;
        [SerializeField] private Transform damageTag;
        [SerializeField] private Transform nameplateTag;


        private Vector3 GetTagPosition(Transform container)
        {
            Vector3 tagPosition = (container != null ? container : defaultTag).position;

            return tagPosition;
        }

        private Transform GetTagTransform(Transform container)
        {
            Transform tagPosition = container != null ? container : defaultTag;

            return tagPosition;
        }


        public Vector3 FindTag(EffectTagType tagType)
        {

            return tagType switch
            {
                EffectTagType.Bottom => GetTagPosition(bottomTag),
                EffectTagType.Foot => GetTagPosition(footTag),
                EffectTagType.Impact => GetTagPosition(impactTag),
                EffectTagType.ImpactStatic => GetTagPosition(impactStaticTag),
                EffectTagType.RightHand => GetTagPosition(rightHandTag),
                EffectTagType.LeftHand => GetTagPosition(leftHandTag),
                _ => throw new ArgumentOutOfRangeException(nameof(tagType)),
            };
        }

        public Vector3 FindNameplateTag() => GetTagPosition(nameplateTag);

        public Vector3 FindDefaultLaunchTag() => FindTag(defaultLaunchTag);

        public void TransferChildren(TagContainer otherContainer)
        {
            TransferChildren(defaultTag, otherContainer.defaultTag);
            TransferChildren(bottomTag, otherContainer.bottomTag);
            TransferChildren(footTag, otherContainer.footTag);
            TransferChildren(impactTag, otherContainer.impactTag);
            TransferChildren(impactStaticTag, otherContainer.impactStaticTag);
            TransferChildren(rightHandTag, otherContainer.rightHandTag);
            TransferChildren(leftHandTag, otherContainer.leftHandTag);
            TransferChildren(damageTag, otherContainer.damageTag);
            TransferChildren(nameplateTag, otherContainer.nameplateTag);
        }

        public void ApplyPositioning(IEffectEntity effectEntity, IEffectPositionerSettings settings)
        {
            Transform targetTag = settings.EffectTagType switch
            {
                EffectTagType.Bottom => GetTagTransform(bottomTag),
                EffectTagType.Foot => GetTagTransform(footTag),
                EffectTagType.Impact => GetTagTransform(impactTag),
                EffectTagType.ImpactStatic => GetTagTransform(impactStaticTag),
                EffectTagType.RightHand => GetTagTransform(rightHandTag),
                EffectTagType.LeftHand => GetTagTransform(leftHandTag),
                _ => throw new ArgumentOutOfRangeException(nameof(settings.EffectTagType)),
            };

            if (settings.AttachToTag)
            {
                effectEntity.Transform.SetParent(targetTag);
            }

            effectEntity.KeepAliveWithNoParticles = settings.KeepAliveWithNoParticles;
            effectEntity.KeepOriginalRotation = settings.KeepOriginalRotation;
            effectEntity.Transform.position = targetTag.position;
        }

        public void ApplyPositioning(FloatingText floatingText)
        {
            floatingText.transform.position = GetTagPosition(damageTag);
        }

        private void TransferChildren(Transform source, Transform destination)
        {
            if (source == destination)
            {
                return;
            }

            while (source.childCount > 0)
            {
                source.GetChild(0).SetParent(destination, false);
            }
        }
    }
}
