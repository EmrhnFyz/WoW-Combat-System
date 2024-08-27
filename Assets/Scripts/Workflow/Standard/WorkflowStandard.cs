using Client;
using Common;
using Core;
using Server;
using UdpKit;
using UnityEngine;

namespace Game.Workflow.Standard
{
    [CreateAssetMenu(fileName = "Workflow Standard Reference", menuName = "Game Data/Scriptable/Workflow Standard", order = 1)]
    internal sealed class WorkflowStandard : ScriptableReference
    {
        [SerializeField] private InterfaceReference interfaceReference;

        private GameManager gameManager;

        protected override void OnRegistered()
        {
            EventHandler.SubscribeEvent<string, NetworkingMode>(GameEvents.GameMapLoaded, OnGameMapLoaded);
            EventHandler.SubscribeEvent<UdpConnectionDisconnectReason>(GameEvents.DisconnectedFromHost, OnDisconnectedFromHost);
            EventHandler.SubscribeEvent(GameEvents.DisconnectedFromMaster, OnDisconnectedFromMaster);

            gameManager = FindObjectOfType<GameManager>();
            interfaceReference.ShowScreen<LobbyScreen, LobbyPanel, LobbyPanel.ShowToken>(new LobbyPanel.ShowToken(true));
        }

        protected override void OnUnregister()
        {
            gameManager = null;

            EventHandler.UnsubscribeEvent<string, NetworkingMode>(GameEvents.GameMapLoaded, OnGameMapLoaded);
            EventHandler.UnsubscribeEvent(GameEvents.DisconnectedFromMaster, OnDisconnectedFromMaster);
            EventHandler.UnsubscribeEvent<UdpConnectionDisconnectReason>(GameEvents.DisconnectedFromHost, OnDisconnectedFromHost);
        }

        private void OnGameMapLoaded(string map, NetworkingMode mode)
        {
            var hasServerLogic = mode is NetworkingMode.Server or NetworkingMode.Both;
            var hasClientLogic = mode is NetworkingMode.Client or NetworkingMode.Both;

            gameManager.CreateWorld(hasServerLogic ? new WorldServer(hasClientLogic) : new WorldClient(false));

            interfaceReference.HideScreen<LobbyScreen>();
            interfaceReference.ShowScreen<BattleScreen, BattleHudPanel>();
        }

        private void OnDisconnectedFromMaster()
        {
            ProcessDisconnect(DisconnectReason.DisconnectedFromMaster);
        }

        private void OnDisconnectedFromHost(UdpConnectionDisconnectReason udpDisconnectReason)
        {
            ProcessDisconnect(udpDisconnectReason.ToDisconnectReason());
        }

        private void ProcessDisconnect(DisconnectReason disconnectReason)
        {
            gameManager.DestroyWorld();

            interfaceReference.HideScreen<BattleScreen>();
            interfaceReference.ShowScreen<LobbyScreen, LobbyPanel, LobbyPanel.ShowToken>(new LobbyPanel.ShowToken(false, disconnectReason));
        }
    }
}
