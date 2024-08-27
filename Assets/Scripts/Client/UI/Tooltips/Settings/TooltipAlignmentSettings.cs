using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TooltipAlignmentSettings
    {
        [SerializeField] private Vector2 pivot;
        [SerializeField] private Vector2 anghoredPosition;
        [SerializeField] private TextAnchor alignment;
        [SerializeField] private uint[] worldCornerIndexes;

        public void Modify(TooltipSlot slot, Vector3[] corners)
        {
            slot.SelfRect.pivot = pivot;
            slot.SelfRect.anchoredPosition += anghoredPosition;
            slot.LayoutGroup.childAlignment = alignment;

            Vector3 worldPosition = Vector3.zero;
            var cornerCount = 0;
            for (var i = 0; i < worldCornerIndexes.Length; i++)
            {
                if (worldCornerIndexes[i] < corners.Length)
                {
                    worldPosition += corners[worldCornerIndexes[i]];
                    cornerCount++;
                }
            }

            if (cornerCount == 0)
            {
                worldPosition = (corners[1] + corners[3]) / 2;
            }
            else
            {
                worldPosition /= cornerCount;
            }

            slot.SelfRect.position = worldPosition;
        }
    }
}
