using Core.Conditions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Client.Conditions
{
    [CreateAssetMenu(fileName = "Player Has Selected Element With Tag", menuName = "Game Data/Conditions/Player/Selected Element With Tag", order = 1)]
    public sealed class PlayerHasSelectedElementWithTag : Condition
    {
        [SerializeField] private string targetTag;

        protected override bool IsApplicable => base.IsApplicable && EventSystem.current.currentSelectedGameObject != null;

        protected override bool IsValid => base.IsValid && EventSystem.current.currentSelectedGameObject.CompareTag(targetTag);
    }
}
