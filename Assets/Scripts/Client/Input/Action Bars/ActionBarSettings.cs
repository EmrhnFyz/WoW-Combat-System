using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Action Bar Settings", menuName = "Player Data/Input/Action Bar", order = 1)]
    public class ActionBarSettings : ScriptableUniqueInfo<ActionBarSettings>
    {
        [SerializeField] private ActionBarSettingsContainer container;
        [SerializeField] private bool saveChanges;
        [SerializeField] private int actionBarSlot;
        [SerializeField] private ClassType classType;
        [SerializeField] private List<SpellInfo> defaultPresets = new();

        private readonly ActionBarData activePreset = new();
        private string PlayerPrefsString => $"{nameof(ActionBarSettings)}#{Id}";

        public new int Id => base.Id;
        public int SlotId => actionBarSlot;
        public ClassType ClassType => classType;
        public IReadOnlyList<ActionButtonData> ActiveButtonPresets => activePreset.Buttons;

        protected override void OnRegister()
        {
            base.OnRegister();

            var loadedFromPrefs = false;
            if (saveChanges)
            {
                var activePresetJson = PlayerPrefs.GetString(PlayerPrefsString, string.Empty);
                if (activePresetJson != string.Empty)
                {
                    JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(PlayerPrefsString, string.Empty), activePreset);
                    loadedFromPrefs = true;
                }
            }

            for (var i = 0; i < InputUtils.ActionBarSlotCount; i++)
            {
                if (i >= activePreset.Buttons.Count)
                {
                    activePreset.Buttons.Add(new ActionButtonData());
                }
            }

            if (!loadedFromPrefs)
            {
                for (var i = 0; i < InputUtils.ActionBarSlotCount && i < defaultPresets.Count; i++)
                {
                    if (defaultPresets[i] != null)
                    {
                        activePreset.Buttons[i].Modify(defaultPresets[i]);
                    }
                }
            }
        }

        protected override void OnUnregister()
        {
            if (saveChanges)
            {
                PlayerPrefs.SetString(PlayerPrefsString, JsonUtility.ToJson(activePreset));
            }

            base.OnUnregister();
        }

        protected override ActionBarSettings Data => this;
        protected override ScriptableUniqueInfoContainer<ActionBarSettings> Container => container;
    }
}
