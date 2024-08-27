using Client.Localization;
using Core;
using System;

namespace Client
{
    public partial class LocalizationReference
    {
        [Serializable]
        private class PowerTypeCostLink
        {
            public SpellPowerType PowerType;
            public LocalizedString LocalizedRawString;
            public LocalizedString LocalizedPercentageString;
        }
    }
}