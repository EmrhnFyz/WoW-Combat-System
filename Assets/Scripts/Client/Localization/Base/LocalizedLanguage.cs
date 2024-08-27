using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Client.Localization
{
    internal class LocalizedLanguage : MonoBehaviour
    {
        [Serializable]
        internal class LocalizationEntry
        {
            public LocalizedString StringReference;
            [TextArea] public string Value;
        }

        [SerializeField] private LocalizedLanguageType languageType;
        [SerializeField] private List<LocalizationEntry> entries;

        internal LocalizedLanguageType LanguageType => languageType;
        internal List<LocalizationEntry> Entries => entries;

        internal void Localize()
        {
            foreach (LocalizationEntry entry in entries)
            {
                entry.StringReference.Value = entry.Value;
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Add missing strings")]
        private void AddMissingStrings()
        {
            var addedStrings = new HashSet<LocalizedString>(entries.Select(item => item.StringReference));

            foreach (var guid in UnityEditor.AssetDatabase.FindAssets($"t:{nameof(LocalizedString)}", null))
            {
                LocalizedString otherString = UnityEditor.AssetDatabase.LoadAssetAtPath<LocalizedString>(UnityEditor.AssetDatabase.GUIDToAssetPath(guid));
                if (!addedStrings.Contains(otherString))
                {
                    entries.Add(new LocalizationEntry { StringReference = otherString });
                }
            }
        }

        [ContextMenu("Populate empty from English")]
        private void PopulateEmptyFromEnglish()
        {
            AddMissingStrings();

            List<LocalizationEntry> englishEntries = Resources.Load<LocalizedLanguage>($"Languages/{LocalizedLanguageType.English}").entries;
            var englishStrings = new Dictionary<LocalizedString, string>();

            englishEntries.ForEach(entry => englishStrings.Add(entry.StringReference, entry.Value));

            foreach (LocalizationEntry entry in entries)
            {
                if (string.IsNullOrEmpty(entry.Value) && englishStrings.ContainsKey(entry.StringReference))
                {
                    entry.Value = englishStrings[entry.StringReference];
                }
            }
        }
#endif
    }
}
