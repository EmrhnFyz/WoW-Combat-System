using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Selection Circle Settings", menuName = "Game Data/Rendering/Selection Circle Settings", order = 1)]
    public class SelectionCircleSettings : ScriptableObject
    {
        [SerializeField] private Color friendlyColor;
        [SerializeField] private Color neutralColor;
        [SerializeField] private Color enemyColor;
        [SerializeField] private EffectTagType targetTag;

        public Color FriendlyColor => friendlyColor;
        public Color NeutralColor => neutralColor;
        public Color EnemyColor => enemyColor;
        public EffectTagType TargetTag => targetTag;
    }
}