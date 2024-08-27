using Common;
using Core;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [CreateAssetMenu(fileName = "Action Bar Settings Container", menuName = "Game Data/Containers/Action Bar Settings", order = 1)]
    public class ActionBarSettingsContainer : ScriptableUniqueInfoContainer<ActionBarSettings>
    {
        [SerializeField] private List<ActionBarSettings> actionBars;

        private readonly Dictionary<(ClassType, int), ActionBarSettings> settingsByClassSlotId = new();

        protected override List<ActionBarSettings> Items => actionBars;

        public IReadOnlyDictionary<(ClassType, int), ActionBarSettings> SettingsByClassSlot => settingsByClassSlotId;

        public override void Register()
        {
            base.Register();

            foreach (ActionBarSettings actionBar in actionBars)
            {
                if (actionBar.ClassType != ClassType.None)
                {
                    settingsByClassSlotId.Add((actionBar.ClassType, actionBar.SlotId), actionBar);
                }
            }
        }

        public override void Unregister()
        {
            settingsByClassSlotId.Clear();

            base.Unregister();

            // save prefabs one time after updating all action bars
            PlayerPrefs.Save();
        }
    }
}
