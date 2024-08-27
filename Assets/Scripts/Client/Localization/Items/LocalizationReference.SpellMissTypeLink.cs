using Client.Localization;
using Core;
using System;

namespace Client
{
    public partial class LocalizationReference
    {
        [Serializable]
        private class SpellMissTypeLink
        {
            public SpellMissType SpellMissType;
            public LocalizedString LocalizedString;
        }
    }
}