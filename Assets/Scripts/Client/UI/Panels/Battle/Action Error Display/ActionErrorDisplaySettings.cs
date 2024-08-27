using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Action Error Display Settings", menuName = "Game Data/Interface/Action Error Display Settings", order = 2)]
    public class ActionErrorDisplaySettings : ScriptableObject
    {
        [SerializeField] private float lifeTime = 3.0f;
        [SerializeField] private float floatingSpeed = 3;
        [SerializeField] private int fontSize = 20;
        [SerializeField] private bool allowRepeating;
        [SerializeField] private AnimationCurve sizeOverTime;
        [SerializeField] private AnimationCurve alphaOverTime;

        public float LifeTime => lifeTime;
        public float FloatingSpeed => floatingSpeed;
        public int FontSize => fontSize;
        public bool AllowRepeating => allowRepeating;
        public AnimationCurve SizeOverTime => sizeOverTime;
        public AnimationCurve AlphaOverTime => alphaOverTime;
    }
}
