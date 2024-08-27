using Common;
using Core;
using Server;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using EventHandler = Common.EventHandler;

namespace Game.Workflow.Dedicated
{
    [CreateAssetMenu(fileName = "Workflow Dedicated Reference", menuName = "Game Data/Scriptable/Workflow Dedicated", order = 1)]
    internal sealed class WorkflowDedicated : ScriptableReference
    {
        [SerializeField] private PhotonBoltReference photon;
        [SerializeField] private DedicatedServerSettings settings;
        [SerializeField] private int maxRestartAttempts = 3;

        private readonly CancellationTokenSource tokenSource = new();
        private GameManager gameManager;
        private int restartCount;

        protected override void OnRegistered()
        {
            gameManager = FindObjectOfType<GameManager>();
            settings.Apply();

            EventHandler.SubscribeEvent<string, NetworkingMode>(GameEvents.GameMapLoaded, OnMapLoaded);
            EventHandler.SubscribeEvent(GameEvents.DisconnectedFromMaster, OnDisconnectedFromMaster);

            StartServer();
        }

        protected override void OnUnregister()
        {
            EventHandler.UnsubscribeEvent<string, NetworkingMode>(GameEvents.GameMapLoaded, OnMapLoaded);
            EventHandler.UnsubscribeEvent(GameEvents.DisconnectedFromMaster, OnDisconnectedFromMaster);

            restartCount = 0;
            tokenSource.Cancel();

            gameManager = null;
        }

        private void StartServer()
        {
            photon.StartServer(new ServerRoomToken("Dedicated Server", "Server", "Lordaeron"), false, OnSuccess, OnFail);

            void OnSuccess()
            {
                Debug.LogWarning("Server start successful!");
            }

            void OnFail()
            {
                Debug.LogWarning("Server start failed!");

                HandleRestart();
            }
        }

        private void OnMapLoaded(string map, NetworkingMode mode)
        {
            Assert.AreEqual(mode, NetworkingMode.Server);

            gameManager.CreateWorld(new WorldServer(false));
        }

        private void OnDisconnectedFromMaster()
        {
            Debug.LogWarning("Disconnected from master!");

            gameManager.DestroyWorld();

            HandleRestart();
        }

        private async void HandleRestart()
        {
            if (restartCount >= maxRestartAttempts)
            {
                Application.Quit();
            }
            else
            {
                Console.WriteLine($"Attempting to restart server! Attempts left: {maxRestartAttempts - restartCount}");

                try
                {
                    await Task.Delay(3000, tokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    return;
                }

                restartCount++;
                StartServer();
            }
        }
    }
}
