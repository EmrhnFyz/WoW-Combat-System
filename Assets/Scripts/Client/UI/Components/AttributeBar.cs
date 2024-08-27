using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Client
{
    public class AttributeBar : UIBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image fillImage;

        public float Ratio { set => slider.value = value; }
        public Image FillImage => fillImage;
    }
}