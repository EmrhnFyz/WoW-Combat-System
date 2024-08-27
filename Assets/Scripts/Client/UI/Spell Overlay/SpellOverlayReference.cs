using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Spell Overlay Reference", menuName = "Game Data/Scriptable/Spell Overlay", order = 10)]
    public class SpellOverlayReference : ScriptableReferenceClient, IScreenHandler<BattleScreen>, IVisibleAuraHandler
    {
        [SerializeField] private InterfaceReference reference;
        [SerializeField] private SpellOverlaySettingsContainer spellOverlaySettingsContainer;

        private readonly Dictionary<int, SpellOverlaySettings> overlaySettingsByAuraId = new();
        private readonly Dictionary<int, List<IVisibleAura>> activeAurasById = new();
        private readonly Dictionary<int, SpellOverlay> activeSpellOverlaysByAuraId = new();

        private BattleScreen battleScreen;

        protected override void OnRegistered()
        {
            base.OnRegistered();

            spellOverlaySettingsContainer.Register();

            for (var i = 0; i < spellOverlaySettingsContainer.ItemList.Count; i++)
            {
                overlaySettingsByAuraId.Add(spellOverlaySettingsContainer.ItemList[i].TriggerAura.Id, spellOverlaySettingsContainer.ItemList[i]);
            }

            reference.AddHandler(this);
        }

        protected override void OnUnregister()
        {
            reference.RemoveHandler(this);

            overlaySettingsByAuraId.Clear();
            spellOverlaySettingsContainer.Unregister();

            Assert.IsTrue(activeAurasById.Count == 0);
            Assert.IsTrue(activeSpellOverlaysByAuraId.Count == 0);

            base.OnUnregister();
        }

        protected override void OnControlStateChanged(Player player, bool underControl)
        {
            if (underControl)
            {
                base.OnControlStateChanged(player, true);

                player.FindBehaviour<AuraControllerClient>().AddHandler(this);
            }
            else
            {
                player.FindBehaviour<AuraControllerClient>().RemoveHandler(this);

                base.OnControlStateChanged(player, false);
            }
        }

        private void HandleSpellOverlay(int auraId, bool addedAura)
        {
            if (battleScreen == null)
            {
                return;
            }

            if (addedAura)
            {
                if (!activeSpellOverlaysByAuraId.ContainsKey(auraId))
                {
                    activeSpellOverlaysByAuraId[auraId] = GameObjectPool.Take(overlaySettingsByAuraId[auraId].Prototype);
                    activeSpellOverlaysByAuraId[auraId].RectTransform.SetParentAndReset(battleScreen.FindTag(BattleHudTagType.SpellOverlay));
                    activeSpellOverlaysByAuraId[auraId].ModifyState(SpellOverlay.State.Active);
                    activeSpellOverlaysByAuraId[auraId].HandleAuraCharges(CalculateTotalCharges(auraId));
                }
            }
            else
            {
                if (activeSpellOverlaysByAuraId.TryGetValue(auraId, out SpellOverlay spellOverlay))
                {
                    activeSpellOverlaysByAuraId[auraId].ModifyState(SpellOverlay.State.Disabled);
                    activeSpellOverlaysByAuraId.Remove(auraId);
                    GameObjectPool.Return(spellOverlay, false);
                }
            }
        }

        private int CalculateTotalCharges(int auraId)
        {
            var charges = 0;
            if (activeAurasById.TryGetValue(auraId, out List<IVisibleAura> auras))
            {
                foreach (IVisibleAura aura in auras)
                {
                    charges += aura.Charges;
                }
            }

            return charges;
        }

        void IScreenHandler<BattleScreen>.OnScreenShown(BattleScreen screen)
        {
            battleScreen = screen;

            foreach (KeyValuePair<int, List<IVisibleAura>> auraEntry in activeAurasById)
            {
                HandleSpellOverlay(auraEntry.Key, true);
            }
        }

        void IScreenHandler<BattleScreen>.OnScreenHide(BattleScreen screen)
        {
            foreach (KeyValuePair<int, List<IVisibleAura>> auraEntry in activeAurasById)
            {
                HandleSpellOverlay(auraEntry.Key, false);
            }

            battleScreen = null;
        }

        void IVisibleAuraHandler.AuraApplied(IVisibleAura visibleAura)
        {
            if (overlaySettingsByAuraId.ContainsKey(visibleAura.AuraId))
            {
                activeAurasById.Insert(visibleAura.AuraId, visibleAura);
                if (battleScreen != null)
                {
                    HandleSpellOverlay(visibleAura.AuraId, true);
                }
            }
        }

        void IVisibleAuraHandler.AuraUnapplied(IVisibleAura visibleAura)
        {
            if (overlaySettingsByAuraId.ContainsKey(visibleAura.AuraId))
            {
                activeAurasById.Delete(visibleAura.AuraId, visibleAura);
                HandleSpellOverlay(visibleAura.AuraId, false);
            }
        }

        void IVisibleAuraHandler.AuraRefreshed(IVisibleAura visibleAura)
        {
            if (activeSpellOverlaysByAuraId.TryGetValue(visibleAura.AuraId, out SpellOverlay spellOverlay))
            {
                spellOverlay.HandleAuraCharges(CalculateTotalCharges(visibleAura.AuraId));
            }
        }
    }
}