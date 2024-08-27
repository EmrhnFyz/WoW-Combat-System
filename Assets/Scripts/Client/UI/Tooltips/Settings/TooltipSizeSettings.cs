using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TooltipSizeSettings
    {
        [SerializeField] private int flexibleWidth;
        [SerializeField] private int preferredWidth;

        public void Modify(TooltipSlot slot)
        {
            slot.LayoutElement.flexibleWidth = flexibleWidth;
            slot.LayoutElement.preferredWidth = preferredWidth;
        }
    }
}
