using Client.UI;
using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class BattleHudPanel : UIWindow<BattleScreen>
    {
        public struct RegisterToken : IPanelRegisterToken<BattleHudPanel>
        {
            public void Initialize(BattleHudPanel panel)
            {
                panel.gameObject.SetActive(false);
            }
        }

        public struct UnregisterToken : IPanelUnregisterToken<BattleHudPanel>
        {
            public void Deinitialize(BattleHudPanel panel)
            {
                panel.gameObject.SetActive(false);
            }
        }

        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private UnitFrame playerUnitFrame;
        [SerializeField] private UnitFrame playerTargetUnitFrame;
        [SerializeField] private UnitFrame playerTargetTargetUnitFrame;
        [SerializeField] private BuffDisplayFrame playerBuffDisplayFrame;
        [SerializeField] private BuffDisplayFrame targetBuffDisplayFrame;
        [SerializeField] private CastFrame playerCastFrame;
        [SerializeField] private ActionErrorDisplay actionErrorDisplay;
        [SerializeField] private List<ActionBar> actionBars;

        private Player localPlayer;

        protected override void PanelInitialized()
        {
            base.PanelInitialized();

            canvasGroup.alpha = 0.0f;
            actionBars.ForEach(actionBar => actionBar.Initialize());
            actionErrorDisplay.Initialize();

            EventHandler.SubscribeEvent<Player, bool>(GameEvents.ClientControlStateChanged, OnControlStateChanged);

            playerCastFrame.UpdateCaster(localPlayer);
            playerUnitFrame.UpdateUnit(localPlayer);
            playerUnitFrame.SetTargetUnitFrame(playerTargetUnitFrame);
            playerUnitFrame.SetBuffDisplayFrame(playerBuffDisplayFrame);
            playerTargetUnitFrame.SetTargetUnitFrame(playerTargetTargetUnitFrame);
            playerTargetUnitFrame.SetBuffDisplayFrame(targetBuffDisplayFrame);
        }

        protected override void PanelDeinitialized()
        {
            EventHandler.UnsubscribeEvent<Player, bool>(GameEvents.ClientControlStateChanged, OnControlStateChanged);

            actionErrorDisplay.Deinitialize();
            actionBars.ForEach(actionBar => actionBar.Denitialize());

            playerUnitFrame.UpdateUnit(null);
            playerTargetUnitFrame.UpdateUnit(null);
            playerBuffDisplayFrame.UpdateUnit(null);
            playerTargetTargetUnitFrame.UpdateUnit(null);
            targetBuffDisplayFrame.UpdateUnit(null);
            playerCastFrame.UpdateCaster(null);

            localPlayer = null;

            base.PanelDeinitialized();
        }

        protected override void PanelUpdated(float deltaTime)
        {
            base.PanelUpdated(deltaTime);

            playerCastFrame.DoUpdate();
            actionErrorDisplay.DoUpdate(deltaTime);
            playerBuffDisplayFrame.DoUpdate();
            targetBuffDisplayFrame.DoUpdate();

            if (localPlayer != null)
            {
                foreach (ActionBar actionBar in actionBars)
                {
                    actionBar.DoUpdate(deltaTime);
                }
            }
        }

        private void OnControlStateChanged(Player player, bool underControl)
        {
            if (underControl)
            {
                localPlayer = player;
                canvasGroup.alpha = 1.0f;

                OnPlayerClassChanged();

                playerUnitFrame.UpdateUnit(localPlayer);
                playerCastFrame.UpdateCaster(localPlayer);

                EventHandler.SubscribeEvent(localPlayer, GameEvents.UnitClassChanged, OnPlayerClassChanged);
            }
            else
            {
                EventHandler.UnsubscribeEvent(localPlayer, GameEvents.UnitClassChanged, OnPlayerClassChanged);

                playerUnitFrame.UpdateUnit(null);
                playerCastFrame.UpdateCaster(null);

                canvasGroup.alpha = 0.0f;
                localPlayer = null;
            }
        }

        private void OnPlayerClassChanged()
        {
            foreach (ActionBar actionBar in actionBars)
            {
                actionBar.ModifyContent(localPlayer.ClassType);
            }
        }
    }
}
