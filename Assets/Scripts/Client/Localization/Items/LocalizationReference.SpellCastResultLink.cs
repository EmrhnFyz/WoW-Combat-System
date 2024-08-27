using Client.Localization;
using Core;
using System;

namespace Client
{
    public partial class LocalizationReference
    {
        [Serializable]
        private class SpellCastResultLink
        {
            public SpellCastResult SpellCastResult;
            public LocalizedString LocalizedString;
        }
    }
}