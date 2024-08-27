using Common;
using TMPro;
using UnityEngine;

namespace Client.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedTextMeshProUGUI : LocalizedBehaviour
    {
        [SerializeField, HideInInspector] private TextMeshProUGUI textMeshPro;

        public TextMeshProUGUI TextMeshPro => textMeshPro;


        private void OnValidate()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();

            Assert.IsNotNull(textMeshPro, $"Broken localization component reference at {this.GetPath()}");
        }

        internal override void Localize()
        {
            textMeshPro.text = LocalizedValue;
        }
    }
}
