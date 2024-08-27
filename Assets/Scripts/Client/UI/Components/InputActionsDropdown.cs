using Client.Localization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client
{
    [RequireComponent(typeof(CustomDropdown))]
    public class InputActionsDropdown : UIBehaviour
    {
        [Serializable]
        private class LocalizedInputActions
        {
            public LocalizedString InputString;
            public InputAction InputAction;
        }

        [SerializeField] private CustomDropdown dropdown;
        [SerializeField] private List<LocalizedInputActions> dropdownItems;

        protected override void Awake()
        {
            base.Awake();

            var localizedOptions = new List<string>();

            foreach (LocalizedInputActions item in dropdownItems)
            {
                localizedOptions.Add(item.InputString.Value);
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(localizedOptions);

            dropdown.OnValueChanged.AddListener(OnDropdownChanged);
        }

        protected override void OnDestroy()
        {
            dropdown.OnValueChanged.RemoveListener(OnDropdownChanged);

            base.OnDestroy();
        }

        private void OnDropdownChanged(int index)
        {
            dropdownItems[index].InputAction.Execute();
        }
    }
}