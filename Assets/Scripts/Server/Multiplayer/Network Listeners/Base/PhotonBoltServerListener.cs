using Bolt;
using Bolt.Utils;
using Common;
using Core;
using UdpKit;
using UnityEngine;

namespace Server
{

    public partial class PhotonBoltServerListener : PhotonBoltBaseListener
    {
        [SerializeField] private BalanceReference balance;
        [SerializeField] private PhotonBoltReference photon;

        private new WorldServer World { get; set; }
        private ServerLaunchState LaunchState { get; set; }
        private ServerRoomToken ServerToken { get; set; }

        public override void Initialize(World world)
        {
            base.Initialize(world);

            World = (WorldServer)world;

            EventHandler.SubscribeEvent<ServerRoomToken>(photon, GameEvents.ServerMapLoaded, OnMapLoaded);
        }

        public override void Deinitialize()
        {
            EventHandler.UnsubscribeEvent<ServerRoomToken>(photon, GameEvents.ServerMapLoaded, OnMapLoaded);

            World = null;

            ServerToken = null;
            LaunchState = 0;

            base.Deinitialize();
        }

        public override void SceneLoadLocalDone(string map, IProtocolToken token)
        {
            if (map == "Launcher")
            {
                return;
            }

            base.SceneLoadLocalDone(map, token);

            if (BoltNetwork.IsConnected)
            {
                World.MapManager.InitializeLoadedMap(1);

                EventHandler.ExecuteEvent(photon, GameEvents.ServerMapLoaded, (ServerRoomToken)token);
            }
        }

        public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
        {
            base.SceneLoadRemoteDone(connection, token);

            World.CreatePlayer(connection);
        }

        public override void SessionCreatedOrUpdated(UdpSession session)
        {
            base.SessionCreatedOrUpdated(session);

            HandleRoomCreation((ServerRoomToken)session.GetProtocolToken());
        }

        public override void ConnectRequest(UdpEndPoint endpoint, IProtocolToken token)
        {
            base.ConnectRequest(endpoint, token);

            if (token is not ClientConnectionToken clientToken || !clientToken.IsValid)
            {
                BoltNetwork.Refuse(endpoint, new ClientRefuseToken(ConnectRefusedReason.InvalidToken));
                return;
            }

            if (clientToken.UnityId == SystemInfo.unsupportedIdentifier)
            {
                BoltNetwork.Refuse(endpoint, new ClientRefuseToken(ConnectRefusedReason.UnsupportedDevice));
                return;
            }

            if (clientToken.Version != ServerToken.Version)
            {
                BoltNetwork.Refuse(endpoint, new ClientRefuseToken(ConnectRefusedReason.InvalidVersion));
                return;
            }

            BoltNetwork.Accept(endpoint);
        }

        public override void Connected(BoltConnection boltConnection)
        {
            base.Connected(boltConnection);

            World.SetDefaultScope(boltConnection);
        }

        public override void Disconnected(BoltConnection boltConnection)
        {
            base.Disconnected(boltConnection);

            World.SetNetworkState(boltConnection, PlayerNetworkState.Disconnected);
        }

        public override void EntityAttached(BoltEntity entity)
        {
            base.EntityAttached(entity);

            World.EntityAttached(entity);
        }

        public override void EntityDetached(BoltEntity entity)
        {
            base.EntityDetached(entity);

            World.EntityDetached(entity);
        }

        private void OnMapLoaded(ServerRoomToken roomToken)
        {
            ProcessServerLaunchState(ServerLaunchState.MapLoaded);

            if (BoltNetwork.IsSinglePlayer)
            {
                HandleRoomCreation(roomToken);
            }
        }

        private void HandleRoomCreation(ServerRoomToken roomToken)
        {
            ServerToken = roomToken;

            ProcessServerLaunchState(ServerLaunchState.SessionCreated);
        }

        private void ProcessServerLaunchState(ServerLaunchState state)
        {
            LaunchState |= state;

            if (LaunchState == ServerLaunchState.Complete)
            {
                World.ServerLaunched(ServerToken);
            }
        }
    }
}
