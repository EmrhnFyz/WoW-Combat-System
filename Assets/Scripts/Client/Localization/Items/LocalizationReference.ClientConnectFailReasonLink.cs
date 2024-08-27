using Client.Localization;
using Core;
using System;

namespace Client
{
    public partial class LocalizationReference
    {
        [Serializable]
        private class ClientConnectFailReasonLink
        {
            public ClientConnectFailReason FailReason;
            public LocalizedString LocalizedString;
        }
    }
}