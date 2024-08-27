using Client.Localization;
using Core;
using TMPro;
using UnityEngine;

public class ChatFrameMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text messageLabel;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private LocalizedString chatGeneralString;

    public RectTransform RectTransform => rectTransform;

    public void Modify(Unit unit, string message)
    {
        messageLabel.text = $"[{chatGeneralString.Value}] [{unit.Name}]: {message}";
    }

    public void MoveToBottom()
    {
        rectTransform.SetAsLastSibling();
    }
}