using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class ActionBar : MonoBehaviour
    {
        [SerializeField] private ActionBarSettingsContainer container;
        [SerializeField] private List<ButtonSlot> buttonSlots;
        [SerializeField] private ActionBarSettings actionBarSettings;

        public void Initialize()
        {
            for (var i = 0; i < buttonSlots.Count; i++)
            {
                buttonSlots[i].Initialize();
            }
        }

        public void Denitialize()
        {
            buttonSlots.ForEach(buttonSlot => buttonSlot.Denitialize());
        }

        public void DoUpdate(float deltaTime)
        {
            foreach (ButtonSlot slot in buttonSlots)
            {
                slot.DoUpdate();
            }
        }

        public void ModifyContent(ClassType classType)
        {
            ActionBarSettings appliedSettings = container.SettingsByClassSlot.TryGetValue((classType, actionBarSettings.SlotId), out ActionBarSettings classSettings)
                ? classSettings
                : actionBarSettings;

            for (var i = 0; i < buttonSlots.Count; i++)
            {
                buttonSlots[i].ButtonContent.UpdateContent(appliedSettings.ActiveButtonPresets[i]);
            }
        }
    }
}