using UnityEngine;

namespace Client
{
    public class InterfaceContainer : MonoBehaviour
    {
        [SerializeField] private RectTransform root;
        [SerializeField] private TransformCanvasTypeDictionary transformByInterfaceCanvasType;

        public RectTransform Root => root;

        public void Register()
        {
            transformByInterfaceCanvasType.Register();
        }

        public void Unregister()
        {
            transformByInterfaceCanvasType.Unregister();
        }

        public RectTransform FindRoot(InterfaceCanvasType canvasType) => transformByInterfaceCanvasType.Value(canvasType);
    }
}
