using Common;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TooltipSettingsBySizeDictionary : SerializedDictionary<TooltipSettingsBySizeDictionary.Entry, TooltipSize, TooltipSizeSettings>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<TooltipSize, TooltipSizeSettings>
        {
            [SerializeField] private TooltipSize tooltipSize;
            [SerializeField] private TooltipSizeSettings tooltipSettings;

            public TooltipSize Key => tooltipSize;
            public TooltipSizeSettings Value => tooltipSettings;
        }
    }
}
