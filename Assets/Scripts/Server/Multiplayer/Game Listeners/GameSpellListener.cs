using Bolt;
using Common;
using Core;
using UnityEngine;

namespace Server
{
    internal class GameSpellListener : BaseGameListener
    {
        internal GameSpellListener(WorldServer world) : base(world)
        {
            EventHandler.SubscribeEvent<SpellDamageInfo>(GameEvents.ServerDamageDone, OnSpellDamageDone);
            EventHandler.SubscribeEvent<SpellHealInfo>(GameEvents.ServerHealingDone, OnSpellHealingDone);
            EventHandler.SubscribeEvent<Unit, SpellInfo, SpellProcessingToken>(GameEvents.ServerSpellLaunch, OnServerSpellLaunch);
            EventHandler.SubscribeEvent<Unit, Unit, SpellInfo, SpellMissType>(GameEvents.ServerSpellHit, OnServerSpellHit);
            EventHandler.SubscribeEvent<Player, Vector3>(GameEvents.ServerPlayerTeleport, OnServerPlayerTeleport);
            EventHandler.SubscribeEvent<Player, SpellCooldown>(GameEvents.ServerSpellCooldown, OnServerSpellCooldown);
            EventHandler.SubscribeEvent<Player, SpellChargeCooldown>(GameEvents.ServerSpellCharge, OnServerSpellChargeCooldown);
        }

        internal void Dispose()
        {
            EventHandler.UnsubscribeEvent<SpellDamageInfo>(GameEvents.ServerDamageDone, OnSpellDamageDone);
            EventHandler.UnsubscribeEvent<SpellHealInfo>(GameEvents.ServerHealingDone, OnSpellHealingDone);
            EventHandler.UnsubscribeEvent<Unit, SpellInfo, SpellProcessingToken>(GameEvents.ServerSpellLaunch, OnServerSpellLaunch);
            EventHandler.UnsubscribeEvent<Unit, Unit, SpellInfo, SpellMissType>(GameEvents.ServerSpellHit, OnServerSpellHit);
            EventHandler.UnsubscribeEvent<Player, Vector3>(GameEvents.ServerPlayerTeleport, OnServerPlayerTeleport);
            EventHandler.UnsubscribeEvent<Player, SpellCooldown>(GameEvents.ServerSpellCooldown, OnServerSpellCooldown);
            EventHandler.UnsubscribeEvent<Player, SpellChargeCooldown>(GameEvents.ServerSpellCharge, OnServerSpellChargeCooldown);
        }

        private void OnSpellDamageDone(SpellDamageInfo damageInfo)
        {
            if (damageInfo.Caster is Player player && World.IsControlledByHuman(player))
            {
                SpellDamageDoneEvent spellDamageEvent = player.IsController
                    ? SpellDamageDoneEvent.Create(GlobalTargets.OnlyServer, ReliabilityModes.ReliableOrdered)
                    : SpellDamageDoneEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);

                spellDamageEvent.Target = damageInfo.Target.BoltEntity.NetworkId;
                spellDamageEvent.DamageAmount = (int)damageInfo.Damage;
                spellDamageEvent.HitType = (int)damageInfo.HitType;
                spellDamageEvent.Send();
            }

            var unitSpellDemageEvent = UnitSpellDamageEvent.Create(damageInfo.Target.BoltEntity, EntityTargets.Everyone);
            unitSpellDemageEvent.CasterId = damageInfo.Caster.BoltEntity.NetworkId;
            unitSpellDemageEvent.Damage = (int)damageInfo.Damage;
            unitSpellDemageEvent.HitType = (int)damageInfo.HitType;
            unitSpellDemageEvent.Send();
        }

        private void OnSpellHealingDone(SpellHealInfo healInfo)
        {
            if (healInfo.Healer is Player player && World.IsControlledByHuman(player))
            {
                SpellHealingDoneEvent spellDamageEvent = player.IsController
                    ? SpellHealingDoneEvent.Create(GlobalTargets.OnlyServer, ReliabilityModes.ReliableOrdered)
                    : SpellHealingDoneEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);

                spellDamageEvent.Target = healInfo.Target.BoltEntity.NetworkId;
                spellDamageEvent.HealAmount = (int)healInfo.Heal;
                spellDamageEvent.IsCrit = healInfo.HasCrit;
                spellDamageEvent.Send();
            }

            // ignore unit healing event, since it currently affects nothing
        }

        private void OnServerSpellLaunch(Unit caster, SpellInfo spellInfo, SpellProcessingToken processingToken)
        {
            var unitCastEvent = UnitSpellLaunchEvent.Create(caster.BoltEntity, EntityTargets.Everyone);
            unitCastEvent.SpellId = spellInfo.Id;
            unitCastEvent.ProcessingEntries = processingToken;
            unitCastEvent.Send();

            SpellCastRequestAnswerEvent spellCastAnswer = caster.IsController
                ? SpellCastRequestAnswerEvent.Create(GlobalTargets.OnlyServer, ReliabilityModes.ReliableOrdered)
                : SpellCastRequestAnswerEvent.Create(caster.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);

            spellCastAnswer.SpellId = spellInfo.Id;
            spellCastAnswer.Result = (int)SpellCastResult.Success;
            spellCastAnswer.ProcessingEntries = processingToken;
            spellCastAnswer.Send();
        }

        private void OnServerSpellHit(Unit caster, Unit target, SpellInfo spellInfo, SpellMissType missType)
        {
            if (missType != SpellMissType.None && caster is Player player && World.IsControlledByHuman(player))
            {
                SpellMissDoneEvent spellMissEvent = player.IsController
                    ? SpellMissDoneEvent.Create(GlobalTargets.OnlyServer, ReliabilityModes.ReliableOrdered)
                    : SpellMissDoneEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);

                spellMissEvent.TargetId = target.BoltEntity.NetworkId;
                spellMissEvent.MissType = (int)missType;
                spellMissEvent.Send();
            }

            var unitSpellHitEvent = UnitSpellHitEvent.Create(target.BoltEntity, EntityTargets.Everyone);
            unitSpellHitEvent.CasterId = caster.BoltEntity.NetworkId;
            unitSpellHitEvent.SpellId = spellInfo.Id;
            unitSpellHitEvent.MissType = (int)missType;
            unitSpellHitEvent.Send();
        }

        private void OnServerPlayerTeleport(Player player, Vector3 targetPosition)
        {
            if (player.BoltEntity.Controller != null)
            {
                var spellTeleportEvent = SpellPlayerTeleportEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);
                spellTeleportEvent.TargetPosition = targetPosition;
                spellTeleportEvent.Send();
            }
        }

        private void OnServerSpellCooldown(Player player, SpellCooldown cooldown)
        {
            if (player.BoltEntity.Controller != null)
            {
                var spellCooldownEvent = SpellCooldownEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);
                spellCooldownEvent.SpellId = cooldown.SpellId;
                spellCooldownEvent.CooldownTime = cooldown.Cooldown;
                spellCooldownEvent.ServerFrame = BoltNetwork.ServerFrame;
                spellCooldownEvent.Send();
            }
        }

        private void OnServerSpellChargeCooldown(Player player, SpellChargeCooldown cooldown)
        {
            if (player.BoltEntity.Controller != null)
            {
                var spellChargeEvent = SpellChargeEvent.Create(player.BoltEntity.Controller, ReliabilityModes.ReliableOrdered);
                spellChargeEvent.SpellId = cooldown.SpellId;
                spellChargeEvent.CooldownTime = cooldown.ChargeTime;
                spellChargeEvent.ServerFrame = BoltNetwork.ServerFrame;
                spellChargeEvent.Send();
            }
        }
    }
}
