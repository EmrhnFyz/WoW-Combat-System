using Core;
using Core.Conditions;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Input Reference", menuName = "Game Data/Scriptable/Input", order = 1)]
    public class InputReference : ScriptableReferenceClient
    {
        [SerializeField] private BalanceReference balance;
        [SerializeField] private TargetingSpellReference spellTargeting;
        [SerializeField] private UnitControllerInputMouseKeyboard unitMouseKeyboardInput;
        [SerializeField] private ActionBarSettingsContainer actionBarSettingsContainer;
        [SerializeField] private List<HotkeyInputItem> hotkeys;
        [SerializeField] private List<InputActionGlobal> globalActions;
        [SerializeField] private List<Condition> inputDisabledWhen;

        public bool IsPlayerInputAllowed { get; private set; }

        protected override void OnRegistered()
        {
            base.OnRegistered();

            actionBarSettingsContainer.Register();
            globalActions.ForEach(globalAction => globalAction.Register());
            hotkeys.ForEach(hotkey => hotkey.Register());
        }

        protected override void OnUnregister()
        {
            hotkeys.ForEach(hotkey => hotkey.Unregister());
            globalActions.ForEach(globalAction => globalAction.Unregister());
            actionBarSettingsContainer.Unregister();

            base.OnUnregister();
        }

        protected override void OnUpdate(float deltaTime)
        {
            IsPlayerInputAllowed = !inputDisabledWhen.Exists(item => item.IsApplicableAndValid());

            foreach (HotkeyInputItem hotkey in hotkeys)
            {
                hotkey.DoUpdate();
            }
        }

        protected override void OnControlStateChanged(Player player, bool underControl)
        {
            if (underControl)
            {
                base.OnControlStateChanged(player, true);

                Player.InputProvider = unitMouseKeyboardInput;
            }
            else
            {
                Player.InputProvider = null;

                base.OnControlStateChanged(player, false);
            }
        }

        public void Say(string message)
        {
            if (!Player.ExistsIn(World))
            {
                return;
            }

            var chatRequest = PlayerChatRequestEvent.Create(Bolt.GlobalTargets.OnlyServer);
            chatRequest.Message = message;
            chatRequest.Send();
        }

        public void SelectTarget(Unit target)
        {
            if (!Player.ExistsIn(World))
            {
                return;
            }

            Player.SetTarget(target);

            var targetSelectionRequest = TargetSelectionRequestEvent.Create(Bolt.GlobalTargets.OnlyServer);
            targetSelectionRequest.TargetId = target?.BoltEntity.NetworkId ?? default;
            targetSelectionRequest.Send();
        }

        public void DoEmote(EmoteType emoteType)
        {
            if (!Player.ExistsIn(World))
            {
                return;
            }

            var emoteRequest = PlayerEmoteRequestEvent.Create(Bolt.GlobalTargets.OnlyServer);
            emoteRequest.EmoteType = (int)emoteType;
            emoteRequest.Send();
        }

        public void SwitchClass(ClassType classType)
        {
            var classRequest = PlayerClassChangeRequestEvent.Create(Bolt.GlobalTargets.OnlyServer);
            classRequest.ClassType = (int)classType;
            classRequest.Send();
        }

        public void CastSpell(int spellId)
        {
            if (!Player.ExistsIn(World))
            {
                return;
            }

            if (balance.SpellInfosById.TryGetValue(spellId, out SpellInfo spellInfo) && spellInfo.ExplicitTargetType == SpellExplicitTargetType.Destination)
            {
                spellTargeting.SelectSpellTargetDestination(spellInfo);
            }
            else
            {
                var spellCastRequest = SpellCastRequestEvent.Create(Bolt.GlobalTargets.OnlyServer);
                spellCastRequest.SpellId = spellId;
                spellCastRequest.MovementFlags = (int)Player.MovementFlags;
                spellCastRequest.Send();
            }
        }

        public void CastSpellWithDestination(int spellId, Vector3 destination)
        {
            if (!Player.ExistsIn(World))
            {
                return;
            }

            var spellCastRequest = SpellCastRequestDestinationEvent.Create(Bolt.GlobalTargets.OnlyServer);
            spellCastRequest.SpellId = spellId;
            spellCastRequest.Destination = destination;
            spellCastRequest.MovementFlags = (int)Player.MovementFlags;
            spellCastRequest.Send();
        }

        public void StopCasting()
        {
            if (!Player.ExistsIn(World))
            {
                return;
            }

            SpellCastCancelRequestEvent.Create(Bolt.GlobalTargets.OnlyServer).Send();
        }

#if UNITY_EDITOR
        [ContextMenu("Validate")]
        private void Validate()
        {
            for (var i = 0; i < hotkeys.Count; i++)
            {
                for (var j = i + 1; j < hotkeys.Count; j++)
                {
                    if (hotkeys[i].HasSameInput(hotkeys[j]))
                    {
                        Debug.LogWarning($"{hotkeys[i].name} has the same input as {hotkeys[j].name}, this combination should be properly prioritized!");
                    }

                    if (hotkeys[i] == hotkeys[j])
                    {
                        Debug.LogError($"{hotkeys[i].name} assigned multiple times!");
                    }
                }
            }
        }

        [ContextMenu("Collect")]
        private void CollectHotkeys()
        {
            hotkeys.Clear();
            globalActions.Clear();

            foreach (var guid in UnityEditor.AssetDatabase.FindAssets($"t:{nameof(HotkeyInputItem)}", null))
            {
                hotkeys.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<HotkeyInputItem>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)));
            }

            foreach (var guid in UnityEditor.AssetDatabase.FindAssets($"t:{nameof(InputActionGlobal)}", null))
            {
                globalActions.Add(UnityEditor.AssetDatabase.LoadAssetAtPath<InputActionGlobal>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid)));
            }
        }
#endif
    }
}
