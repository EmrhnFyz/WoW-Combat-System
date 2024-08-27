using Common;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TooltipSettingsByAlignmentDictionary : SerializedDictionary<TooltipSettingsByAlignmentDictionary.Entry, TooltipAlignment, TooltipAlignmentSettings>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<TooltipAlignment, TooltipAlignmentSettings>
        {
            [SerializeField] private TooltipAlignment tooltipAlignment;
            [SerializeField] private TooltipAlignmentSettings tooltipSettings;

            public TooltipAlignment Key => tooltipAlignment;
            public TooltipAlignmentSettings Value => tooltipSettings;
        }
    }
}
