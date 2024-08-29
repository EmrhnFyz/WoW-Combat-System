using Common;
using Fusion;
using UnityEngine;

namespace Core
{
    public abstract class Entity : NetworkBehaviour
    {
        [SerializeField, Header(nameof(Entity))] private BalanceReference balance;

        protected BalanceReference Balance => balance;
        protected bool IsValid { get; private set; }

        internal World World { get; private set; }
        internal abstract bool AutoScoped { get; }

        public NetworkObject BoltEntity => Object;
        public bool IsOwner => Object.HasStateAuthority;
        public bool IsController => Object.HasInputAuthority;

        private void Awake() => EventHandler.SubscribeEvent<bool, World>(gameObject, GameEvents.EntityPooled, OnEntityPooled);

        private void OnDestroy() => EventHandler.UnsubscribeEvent<bool, World>(gameObject, GameEvents.EntityPooled, OnEntityPooled);

        public override void Spawned()
        {
            base.Spawned();

            IsValid = true;
        }

        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            IsValid = false;

            base.Despawned(runner, hasState);
        }

        internal virtual void DoUpdate(int deltaTime)
        {
        }

        private void OnEntityPooled(bool isTaken, World world) => World = isTaken ? world : null;
    }
}