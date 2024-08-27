using System;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Unit Movement Definition", menuName = "Game Data/Physics/Unit Movement Definition", order = 4)]
    public class UnitMovementDefinition : ScriptableObject
    {
        [SerializeField] private float walkSpeed = 3.0f;
        [SerializeField] private float runSpeed = 7.0f;
        [SerializeField] private float runBackSpeed = 4.5f;

        public float BaseSpeedByType(UnitMoveType moveType)
        {
            return moveType switch
            {
                UnitMoveType.Walk => walkSpeed,
                UnitMoveType.Run => runSpeed,
                UnitMoveType.RunBack => runBackSpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(moveType), moveType, "Unknown movement type!"),
            };
        }
    }
}
