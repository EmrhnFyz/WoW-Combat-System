﻿using System;
using UdpKit;

namespace Core
{
    public interface IPhotonBoltController
    {
        Map<Guid, UdpSession> Sessions { get; }
        string Version { get; }

        void StartServer(ServerRoomToken serverToken, bool withClientLogic, Action onStartSuccess, Action onStartFail);
        void StartSinglePlayer(ServerRoomToken serverToken, Action onStartSuccess, Action onStartFail);
        void StartConnection(UdpSession session, ClientConnectionToken token, Action onConnectSuccess, Action<ClientConnectFailReason> onConnectFail);
        void StartClient(Action onStartSuccess, Action onStartFail, bool forceRestart);
    }
}
