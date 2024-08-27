using Bolt;
using Common;
using Core;

namespace Server
{
    internal class GamePlayerListener : BaseGameListener
    {
        internal GamePlayerListener(WorldServer world) : base(world)
        {
            EventHandler.SubscribeEvent<Player, UnitMoveType, float>(GameEvents.ServerPlayerSpeedChanged, OnPlayerSpeedChanged);
            EventHandler.SubscribeEvent<Player, bool>(GameEvents.ServerPlayerRootChanged, OnPlayerRootChanged);
            EventHandler.SubscribeEvent<Player, bool>(GameEvents.ServerPlayerMovementControlChanged, OnPlayerMovementControlChanged);
        }

        internal void Dispose()
        {
            EventHandler.UnsubscribeEvent<Player, UnitMoveType, float>(GameEvents.ServerPlayerSpeedChanged, OnPlayerSpeedChanged);
            EventHandler.UnsubscribeEvent<Player, bool>(GameEvents.ServerPlayerRootChanged, OnPlayerRootChanged);
            EventHandler.UnsubscribeEvent<Player, bool>(GameEvents.ServerPlayerMovementControlChanged, OnPlayerMovementControlChanged);
        }

        private void OnPlayerSpeedChanged(Player player, UnitMoveType moveType, float rate)
        {
            if (player.BoltEntity.Controller != null)
            {
                var speedChangeEvent = PlayerSpeedRateChangedEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);
                speedChangeEvent.MoveType = (int)moveType;
                speedChangeEvent.SpeedRate = rate;
                speedChangeEvent.Send();
            }
        }

        private void OnPlayerRootChanged(Player player, bool applied)
        {
            if (player.BoltEntity.Controller != null)
            {
                var rootChangedEvent = PlayerRootChangedEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);
                rootChangedEvent.Applied = applied;
                rootChangedEvent.Send();
            }
        }

        private void OnPlayerMovementControlChanged(Player player, bool hasControl)
        {
            if (player.BoltEntity.Controller != null)
            {
                var movementControlEvent = PlayerMovementControlChanged.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);
                movementControlEvent.PlayerHasControl = hasControl;
                movementControlEvent.LastServerPosition = player.Position;
                movementControlEvent.LastServerMovementFlags = (int)player.MovementFlags;
                movementControlEvent.Send();
            }
        }
    }
}
