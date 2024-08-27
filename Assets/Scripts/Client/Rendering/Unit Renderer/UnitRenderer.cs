using Bolt;
using Common;
using Core;
using UnityEngine;

namespace Client
{
    public sealed partial class UnitRenderer : EntityEventListener<IUnitState>
    {
        [SerializeField] private RenderingReference rendering;
        [SerializeField] private TagContainer dummyTagContainer;
        [SerializeField] private UnitSoundController soundController;

        private Vector3 targetSmoothVelocity;

        private readonly AuraEffectController auraEffectController = new();
        private UnitModel model;
        private bool canAnimate = true;

        public TagContainer TagContainer => model == null ? dummyTagContainer : model.TagContainer;
        public Unit Unit { get; private set; }

        public void Attach(Unit unit)
        {
            Unit = unit;
            transform.position = Unit.Position;

            ReplaceModel(Unit.Model, UnitModelReplacementMode.ScopeIn);
            OnScaleChanged();

            Unit.BoltEntity.AddEventListener(this);

            Unit.AddCallback(nameof(IUnitState.DeathState), OnDeathStateChanged);
            Unit.AddCallback(nameof(IUnitState.SpellCast), OnSpellCastChanged);
            Unit.AddCallback(nameof(IUnitState.EmoteType), OnEmoteTypeChanged);
            Unit.AddCallback(nameof(IUnitState.EmoteFrame), OnEmoteFrameChanged);

            EventHandler.SubscribeEvent(Unit, GameEvents.UnitModelChanged, OnModelChanged);
            EventHandler.SubscribeEvent(Unit, GameEvents.UnitScaleChanged, OnScaleChanged);
            EventHandler.SubscribeEvent(Unit, GameEvents.UnitVisualsChanged, OnVisualsChanged);

            auraEffectController.HandleAttach(this);
        }

        public UnitModel Detach(UnitModelReplacementMode mode)
        {
            auraEffectController.HandleDetach();

            Unit.BoltEntity.RemoveEventListener(this);

            Unit.RemoveCallback(nameof(IUnitState.DeathState), OnDeathStateChanged);
            Unit.RemoveCallback(nameof(IUnitState.SpellCast), OnSpellCastChanged);
            Unit.RemoveCallback(nameof(IUnitState.EmoteType), OnEmoteTypeChanged);
            Unit.RemoveCallback(nameof(IUnitState.EmoteFrame), OnEmoteFrameChanged);

            EventHandler.UnsubscribeEvent(Unit, GameEvents.UnitModelChanged, OnModelChanged);
            EventHandler.UnsubscribeEvent(Unit, GameEvents.UnitScaleChanged, OnScaleChanged);
            EventHandler.UnsubscribeEvent(Unit, GameEvents.UnitVisualsChanged, OnVisualsChanged);

            CancelInvoke();
            Unit = null;

            return ReplaceModel(mode: mode);
        }

        public void DoUpdate(float deltaTime)
        {
            transform.SetPositionAndRotation(Vector3.SmoothDamp(transform.position, Unit.Position, ref targetSmoothVelocity, rendering.UnitRendererSettings.RenderInterpolationSmoothTime), Unit.Rotation);

            if (model != null)
            {
                model.DoUpdate(this, deltaTime);
            }
        }

        public override void OnEvent(UnitSpellLaunchEvent launchEvent)
        {
            base.OnEvent(launchEvent);

            if (!Unit.IsController)
            {
                var token = launchEvent.ProcessingEntries as SpellProcessingToken;
                EventHandler.ExecuteEvent(GameEvents.SpellLaunched, Unit, launchEvent.SpellId, token);
            }
        }

        public override void OnEvent(UnitSpellDamageEvent spellDamageEvent)
        {
            base.OnEvent(spellDamageEvent);

            if (canAnimate)
            {
                var hitType = (HitType)spellDamageEvent.HitType;

                if (model != null)
                {
                    model.Animator.SetBool("WoundedCrit", hitType.HasTargetFlag(HitType.CriticalHit));
                    model.Animator.SetTrigger("Wound");
                }
            }
        }

        public override void OnEvent(UnitSpellHitEvent spellHitEvent)
        {
            base.OnEvent(spellHitEvent);

            EventHandler.ExecuteEvent(GameEvents.SpellHit, Unit, spellHitEvent.SpellId);
        }

        public void TriggerInstantCast(SpellInfo spellInfo)
        {
            if (canAnimate && model != null)
            {
                model.TriggerInstantCast(spellInfo);
            }
        }

        private void UpdateAnimationState(bool enabled)
        {
            canAnimate = enabled;
            if (model != null)

            {
                model.Animator.speed = canAnimate ? 1.0f : 0.0f;
            }
        }

        private void OnModelChanged() => ReplaceModel(Unit.Model, UnitModelReplacementMode.Transformation);

        private void OnScaleChanged() => transform.localScale = new Vector3(Unit.Scale, Unit.Scale, Unit.Scale);

        private void OnVisualsChanged() => HandleVisualEffects(false);

        private void ReplaceModel(int modelId, UnitModelReplacementMode mode)
        {
            Assert.IsTrue(mode != UnitModelReplacementMode.ScopeOut);

            if (model != null && model.Settings.Id == modelId)
            {
                return;
            }

            if (rendering.Models.TryGetValue(modelId, out UnitModelSettings newModelSettings))
            {
                UnitModel newModel = GameObjectPool.Take(newModelSettings.Prototype);
                var modelInitializer = new UnitModelInitializer
                {
                    UnitRenderer = this,
                    ModelSettings = newModelSettings,
                    PreviousModel = model,
                    ReplacementMode = mode
                };

                newModel.Initialize(modelInitializer);
                ReplaceModel(newModel);
            }
            else
            {
                Debug.LogError($"Missing model with id: {modelId}");
            }
        }

        private UnitModel ReplaceModel(UnitModel newModel = null, UnitModelReplacementMode mode = UnitModelReplacementMode.ScopeIn)
        {
            UnitModel oldModel = model;
            if (model != null && newModel != null)
            {
                model.TagContainer.TransferChildren(newModel.TagContainer);
            }

            model = newModel;
            UpdateAnimationState(canAnimate);
            soundController.HandleModelChange(model);

            if (oldModel != null)
            {
                EventHandler.ExecuteEvent(rendering, GameEvents.UnitModelAttached, oldModel, this, false);
            }

            if (newModel != null)
            {
                EventHandler.ExecuteEvent(rendering, GameEvents.UnitModelAttached, newModel, this, true);
            }

            if (oldModel != null && mode != UnitModelReplacementMode.ScopeOut)
            {
                oldModel.Deinitialize();
                oldModel = null;
            }

            return oldModel;
        }

        private void HandleVisualEffects(bool instantly) => model?.HandleVisualEffects(this, instantly);

        private void HandleEmoteUpdate()
        {
            EmoteType emoteType = Unit.EmoteType;
            if (emoteType.IsState() || emoteType == EmoteType.None)
            {
                DoEmote();
            }
            else if (emoteType.IsOneShot() && BoltNetwork.Frame - Unit.EmoteFrame <= UnitUtils.EmoteOneShotFrameThreshold)
            {
                DoEmote();
            }
        }

        private void DoEmote(float cancellationDelay = 0.2f)
        {
            CancelInvoke(nameof(ResetEmoteTrigger));

            soundController.HandleEmote(Unit.EmoteType);

            if (model != null)
            {
                model.Animator.SetTrigger("Emote Trigger");
                model.Animator.SetInteger("Emote", (int)Unit.EmoteType);
            }

            Invoke(nameof(ResetEmoteTrigger), cancellationDelay);
        }

        private void ResetEmoteTrigger()
        {
            if (model != null)
            {
                model.Animator.ResetTrigger("Emote Trigger");
            }
        }

        private void OnDeathStateChanged()
        {
            if (model != null)
            {
                model.Animator.SetBool("IsDead", Unit.IsDead);
            }

            if (Unit.IsDead)
            {
                soundController.PlayOneShot(UnitSounds.Death);
            }
        }

        private void OnSpellCastChanged()
        {
            if (model != null)
            {
                model.Animator.SetBool("Casting", Unit.SpellCast.State.Id != 0);
            }
        }

        private void OnEmoteTypeChanged()
        {
            HandleEmoteUpdate();
        }

        private void OnEmoteFrameChanged()
        {
            HandleEmoteUpdate();
        }
    }
}