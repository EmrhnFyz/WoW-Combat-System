using UnityEngine;
using UnityEngine.EventSystems;

public class AnimatorPointerNotifier : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator animator;
    [SerializeField] private string parameter;

    private void OnDisable() => animator.SetBool(parameter, false);

    public void OnPointerEnter(PointerEventData eventData) => animator.SetBool(parameter, true);

    public void OnPointerExit(PointerEventData eventData) => animator.SetBool(parameter, false);
}