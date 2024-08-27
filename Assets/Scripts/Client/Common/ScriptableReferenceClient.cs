using Common;
using Core;

namespace Client
{
    public abstract class ScriptableReferenceClient : ScriptableReference
    {
        protected World World { get; private set; }
        public Player Player { get; private set; }

        protected override void OnRegistered()
        {
            EventHandler.SubscribeEvent<World, bool>(GameEvents.WorldStateChanged, OnWorldStateChanged);
            EventHandler.SubscribeEvent<Player, bool>(GameEvents.ClientControlStateChanged, OnControlStateChanged);
        }

        protected override void OnUnregister()
        {
            EventHandler.UnsubscribeEvent<Player, bool>(GameEvents.ClientControlStateChanged, OnControlStateChanged);
            EventHandler.UnsubscribeEvent<World, bool>(GameEvents.WorldStateChanged, OnWorldStateChanged);
        }

        protected virtual void OnWorldStateChanged(World world, bool created)
        {
            World = created ? world : null;
        }

        protected virtual void OnControlStateChanged(Player player, bool underControl)
        {
            Player = underControl ? player : null;
        }
    }
}
