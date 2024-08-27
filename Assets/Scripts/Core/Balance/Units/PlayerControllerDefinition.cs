using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Player Controller Definition", menuName = "Game Data/Physics/Player Controller Definition", order = 3)]
    public class PlayerControllerDefinition : ScriptableObject
    {
        [SerializeField] private float jumpSpeed = 4.0f;
        [SerializeField] private float rotateSpeed = 250.0f;
        [SerializeField] private float baseGroundCheckDistance = 0.2f;

        public float JumpSpeed => jumpSpeed;
        public float RotateSpeed => rotateSpeed;
        public float BaseGroundCheckDistance => baseGroundCheckDistance;
    }
}
