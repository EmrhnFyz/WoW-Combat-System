using Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class LobbyMapSlot : MonoBehaviour
    {
        [SerializeField] private Button slotButton;
        [SerializeField] private Image selectedFrame;
        [SerializeField] private Image mapFrame;

        public event Action<LobbyMapSlot> EventLobbyMapSlotSelected;

        public MapDefinition MapDefinition { get; private set; }

        public void Initialize(MapDefinition mapDefiniton)
        {
            gameObject.SetActive(true);

            MapDefinition = mapDefiniton;
            mapFrame.sprite = mapDefiniton.SlotBackground;
            slotButton.interactable = MapDefinition.IsAvailable;

            slotButton.onClick.AddListener(OnMapSlotClicked);
        }

        public void Deinitialize()
        {
            MapDefinition = null;

            slotButton.onClick.RemoveListener(OnMapSlotClicked);
        }

        public void SetSelectState(bool isSelected)
        {
            selectedFrame.enabled = isSelected;
        }

        public void Select()
        {
            EventLobbyMapSlotSelected?.Invoke(this);
        }

        private void OnMapSlotClicked()
        {
            Select();
        }
    }
}
