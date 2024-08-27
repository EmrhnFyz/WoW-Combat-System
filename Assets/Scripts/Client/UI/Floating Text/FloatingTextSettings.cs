using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Floating Text Settings", menuName = "Game Data/Interface/Floating Text Settings", order = 1)]
    public class FloatingTextSettings : ScriptableObject
    {
        [SerializeField] private float lifeTime = 3.0f;
        [SerializeField] private float randomOffset = 0.2f;
        [SerializeField] private float floatingSpeed = 3;
        [SerializeField] private int fontSize = 120;
        [SerializeField] private Color fontColor;
        [SerializeField] private Material fontMaterial;
        [SerializeField] private AnimationCurve sizeOverTime;
        [SerializeField] private AnimationCurve alphaOverTime;
        [SerializeField] private AnimationCurve sizeOverDistanceToCamera;
        [SerializeField] private AnimationCurve randomOffsetOverDistance;

        public float LifeTime => lifeTime;
        public float RandomOffset => randomOffset;
        public float FloatingSpeed => floatingSpeed;
        public int FontSize => fontSize;
        public Color FontColor => fontColor;
        public Material FontMaterial => fontMaterial;
        public AnimationCurve SizeOverTime => sizeOverTime;
        public AnimationCurve AlphaOverTime => alphaOverTime;
        public AnimationCurve SizeOverDistanceToCamera => sizeOverDistanceToCamera;
        public AnimationCurve RandomOffsetOverDistance => randomOffsetOverDistance;
    }
}
