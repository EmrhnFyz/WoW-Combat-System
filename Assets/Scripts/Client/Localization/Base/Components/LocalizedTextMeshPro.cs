using Common;
using TMPro;
using UnityEngine;

namespace Client.Localization
{
    [RequireComponent(typeof(TextMeshPro))]
    public class LocalizedTextMeshPro : LocalizedBehaviour
    {
        [SerializeField, HideInInspector] private TextMeshPro textMeshPro;

        private void OnValidate()
        {
            textMeshPro = GetComponent<TextMeshPro>();

            Assert.IsNotNull(textMeshPro, $"Broken localization component reference at {this.GetPath()}");
        }

        internal override void Localize()
        {
            textMeshPro.text = LocalizedValue;
        }
    }
}
