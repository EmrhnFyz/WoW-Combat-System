﻿using Bolt.Utils;
using Common;
using System;
using UdpKit;
using UnityEngine;

namespace Core
{
    public abstract class WorldEntity : Entity
    {
        public new abstract class CreateToken : Entity.CreateToken
        {
            public Vector3 Position { get; set; }
            public Quaternion Rotation { get; set; }

            public override void Read(UdpPacket packet)
            {
                Position = packet.ReadVector3();
                Rotation = packet.ReadQuaternion();
            }

            public override void Write(UdpPacket packet)
            {
                packet.WriteVector3(Position);
                packet.WriteQuaternion(Rotation);
            }
        }

        private IWorldEntityState worldEntityState;
        private CreateToken createToken;

        internal MapGrid.Cell CurrentCell { get; set; }

        public Vector3 Position { get => transform.position; set => transform.position = value; }
        public Quaternion Rotation { get => transform.rotation; set => transform.rotation = value; }

        public abstract string Name { get; internal set; }
        public virtual float Size { get; } = StatUtils.DefaultEntitySize;

        public Map Map { get; private set; }

        public bool IsVisible { get; } = true;
        public bool IsVisibilityChanged { get; internal set; }
        public int StealthSubtlety { get; internal set; }
        public int StealthDetection { get; internal set; }
        public int InvisibilityPower { get; internal set; }
        public int InvisibilityDetection { get; internal set; }

        public override void Attached()
        {
            base.Attached();

            worldEntityState = entity.GetState<IWorldEntityState>();
            worldEntityState.SetTransforms(worldEntityState.Transform, transform);

            createToken = (CreateToken)entity.AttachToken;
            Position = createToken.Position;
            Rotation = createToken.Rotation;
        }

        public override void Detached()
        {
            worldEntityState.SetTransforms(worldEntityState.Transform, null);
            worldEntityState = null;
            createToken = null;

            base.Detached();
        }

        internal virtual void PrepareForScoping()
        {
            if (IsOwner)
            {
                createToken.Position = Position;
                createToken.Rotation = Rotation;
            }
        }

        internal virtual void UpdateVisibility(bool forced)
        {
            IsVisibilityChanged = true;
        }

        internal void UpdateSyncTransform(bool shouldSync)
        {
            worldEntityState.SetTransforms(worldEntityState.Transform, shouldSync ? transform : null);
        }

        internal void SetMap(Map map)
        {
            Assert.IsNotNull(map);
            Assert.IsTrue(IsValid);

            if (Map == map)
            {
                return;
            }

            Map = map;
            Map.AddWorldEntity(this);
        }

        internal void ResetMap()
        {
            Map.RemoveWorldEntity(this);

            Map = null;
        }

        public bool CanSeeOrDetect(WorldEntity target, bool ignoreStealth = false, bool checkDistance = false)
        {
            if (this == target)
            {
                return true;
            }

            if (target.IsNeverVisibleFor(this) || CanNeverSee(target))
            {
                return false;
            }

            if (target.IsAlwaysVisibleFor(this) || CanAlwaysSee(target))
            {
                return true;
            }

            if (checkDistance && !IsWithinDistance(target, Map.VisibilityRange, false))
            {
                return false;
            }

            if (!ignoreStealth && !CanDetectInvisibility())
            {
                return false;
            }

            if (!ignoreStealth && !CanDetectStealth())
            {
                return false;
            }

            return true;

            bool CanDetectInvisibility()
            {
                return InvisibilityDetection >= target.InvisibilityPower;
            }

            bool CanDetectStealth()
            {
                if (target.StealthSubtlety <= 0)
                {
                    return true;
                }

                var distance = ExactDistanceTo(target);
                var combatReach = 0.0f;

                if (this is Unit unit)
                {
                    if (unit.Auras.HasAuraType(AuraEffectType.DetectAllStealth))
                    {
                        return true;
                    }

                    combatReach = StatUtils.DefaultCombatReach;
                }

                if (distance < combatReach)
                {
                    return true;
                }

                if (!IsFacing(target, SpellTargetDirections.Front, 90.0f, combatReach))
                {
                    return false;
                }

                var detectionValue = 30 + StealthDetection - target.StealthSubtlety;
                var visibilityRange = (detectionValue * 0.3f) + combatReach;

                if (distance > visibilityRange)
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsNeverVisibleFor(WorldEntity observer)
        {
            return false;
        }

        public bool IsAlwaysVisibleFor(WorldEntity observer)
        {
            return this == observer;
        }

        public bool CanNeverSee(WorldEntity target)
        {
            return Map != target.Map;
        }

        public bool CanAlwaysSee(WorldEntity target)
        {
            return this == target;
        }

        public bool IsFacing(WorldEntity target, SpellTargetDirections direction, float angle, float backBuffer = StatUtils.DefaultCombatReach)
        {
            Vector3 facingDirection = direction switch
            {
                SpellTargetDirections.Front => transform.forward,
                SpellTargetDirections.Back => -transform.forward,
                SpellTargetDirections.Right => transform.right,
                SpellTargetDirections.Left => -transform.right,
                SpellTargetDirections.FrontRight => transform.forward + transform.right,
                SpellTargetDirections.BackRight => -transform.forward + transform.right,
                SpellTargetDirections.BackLeft => -transform.forward - transform.right,
                SpellTargetDirections.FrontLeft => transform.forward - transform.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "Unknown direction type!"),
            };
            var projectedFacingDirection = Vector3.ProjectOnPlane(facingDirection, Vector3.up);
            Vector3 pointOfView = Position - (projectedFacingDirection.normalized * backBuffer);
            var targetDirection = Vector3.ProjectOnPlane(target.Position - pointOfView, Vector3.up);
            return Vector3.Angle(targetDirection, projectedFacingDirection) < angle;
        }

        public bool IsWithinDistance(WorldEntity target, float range, bool is3D)
        {
            var sizeDistance = Size + target.Size;
            var actualRange = range + sizeDistance;
            Vector3 position = Position;
            Vector3 targetPosition = target.Position;

            var dx = position.x - targetPosition.x;
            var dz = position.z - targetPosition.z;
            var sqrDistance = (dx * dx) + (dz * dz);

            if (is3D)
            {
                var dy = position.y - targetPosition.y;
                sqrDistance += dy * dy;
            }

            return sqrDistance < actualRange * actualRange;
        }

        public float ExactDistanceTo(Vector3 position)
        {
            return Vector3.Distance(Position, position);
        }

        public float ExactDistanceSqrTo(Vector3 position)
        {
            return Vector3.SqrMagnitude(Position - position);
        }

        public float ExactDistanceTo(WorldEntity target)
        {
            return ExactDistanceTo(target.Position);
        }

        public float ExactDistanceSqrTo(WorldEntity target)
        {
            return ExactDistanceTo(target.Position);
        }

        public void SetFacingTo(WorldEntity target)
        {
            SetFacingTo(target.Position);
        }

        public void SetFacingTo(Vector3 position)
        {
            Rotation = Quaternion.LookRotation(position - Position);
        }
    }
}