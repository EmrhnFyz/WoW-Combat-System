using Client;
using Common;
using Core;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChatFrame : MonoBehaviour
{
    [SerializeField] private InputReference input;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private ChatFrameMessage messagePrototype;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Transform messageContainer;
    [SerializeField] private HotkeyInputItem chatFocusHotkey;
    [SerializeField] private int maxMessageCount = 100;

    private readonly List<ChatFrameMessage> chatMessages = new();
    private const float BottomSnapThreshold = 0.001f;

    public bool SnapToBottom { get; set; } = true;

    private void Awake()
    {
        EventHandler.SubscribeEvent<Unit, string>(GameEvents.UnitChat, OnUnitChat);
        EventHandler.SubscribeEvent<HotkeyState>(chatFocusHotkey, GameEvents.HotkeyStateChanged, OnHotkeyStateChanged);

        inputField.onSubmit.AddListener(OnSubmit);
        inputField.onDeselect.AddListener(OnDeselect);

        GameObjectPool.PreInstantiate(messagePrototype, maxMessageCount);
    }

    private void OnDestroy()
    {
        foreach (ChatFrameMessage message in chatMessages)
        {
            GameObjectPool.Return(message, true);
        }

        chatMessages.Clear();
        inputField.onSubmit.RemoveListener(OnSubmit);
        inputField.onDeselect.RemoveListener(OnDeselect);

        EventHandler.UnsubscribeEvent<Unit, string>(GameEvents.UnitChat, OnUnitChat);
        EventHandler.UnsubscribeEvent<HotkeyState>(chatFocusHotkey, GameEvents.HotkeyStateChanged, OnHotkeyStateChanged);
    }

    private void Update()
    {
        if (SnapToBottom && scrollRect.verticalNormalizedPosition > BottomSnapThreshold)
        {
            scrollRect.normalizedPosition = Vector2.zero;
        }
    }

    private void OnDeselect(string text)
    {
        inputField.text = string.Empty;
    }

    private void OnSubmit(string text)
    {
        inputField.text = string.Empty;

        if (!inputField.wasCanceled && !string.IsNullOrEmpty(text))
        {
            input.Say(text);
        }

        if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OnUnitChat(Unit unit, string text)
    {
        ChatFrameMessage chatFrameMessage;
        if (chatMessages.Count >= maxMessageCount)
        {
            chatFrameMessage = chatMessages[0];
            chatMessages.RemoveAt(0);
        }
        else
        {
            chatFrameMessage = GameObjectPool.Take(messagePrototype);
            chatFrameMessage.RectTransform.SetParent(messageContainer, false);
        }

        chatMessages.Add(chatFrameMessage);
        chatFrameMessage.Modify(unit, text);
        chatFrameMessage.MoveToBottom();

        if (scrollRect.verticalNormalizedPosition < BottomSnapThreshold)
        {
            SnapToBottom = true;
        }
    }

    private void OnHotkeyStateChanged(HotkeyState state)
    {
        if (enabled && state == HotkeyState.Pressed)
        {
            if (EventSystem.current.currentSelectedGameObject != inputField.gameObject)
            {
                EventSystem.current.SetSelectedGameObject(inputField.gameObject);
            }
        }
    }
}