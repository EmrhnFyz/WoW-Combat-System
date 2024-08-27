using UnityEngine;

namespace Client
{
    public class ComboPointSlot : MonoBehaviour
    {
        [SerializeField] private CanvasGroup activeCanvasGroup;

        public void ModifyState(bool active)
        {
            activeCanvasGroup.alpha = active ? 1.0f : 0.0f;
        }
    }
}