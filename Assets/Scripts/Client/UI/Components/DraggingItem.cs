using UnityEngine;

namespace Client
{
    public class DraggingItem : MonoBehaviour
    {
        [SerializeField] private ButtonContent draggingContent;
        [SerializeField] private RectTransform rectTransform;

        public RectTransform RectTransform => rectTransform;

        private void Awake()
        {
            draggingContent.ContentImage.sprite = null;
            draggingContent.enabled = false;
        }

        public void DragContent(ButtonContent buttonContent)
        {
            gameObject.SetActive(true);
            draggingContent.FromDrag(buttonContent);
        }

        public void DropContent(ButtonContent targetButtonContent)
        {
            targetButtonContent.FromDrop(draggingContent);
            gameObject.SetActive(false);
        }

        public void DropReplaceContent(ButtonContent targetButtonContent)
        {
            targetButtonContent.Replace(draggingContent);
        }
    }
}
