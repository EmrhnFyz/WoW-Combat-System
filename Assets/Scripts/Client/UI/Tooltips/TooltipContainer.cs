using UnityEngine;

namespace Client
{
    public class TooltipContainer : MonoBehaviour
    {
        [SerializeField] private TooltipItemNormal tooltipNormal;
        [SerializeField] private TooltipItemSpell tooltipSpell;

        public static string ContainerTag => "Tooltip Container";

        public TooltipItemNormal TooltipNormal => tooltipNormal;
        public TooltipItemSpell TooltipSpell => tooltipSpell;
    }
}
