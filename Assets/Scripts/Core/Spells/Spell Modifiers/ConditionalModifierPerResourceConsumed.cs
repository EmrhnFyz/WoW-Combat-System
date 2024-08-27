using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Conditional Modifier - Per Resource Consumed", menuName = "Game Data/Spells/Conditional Modifiers/Per Resource Consumed", order = 1)]
    public class ConditionalModifierPerResourceConsumed : ConditionalModiferValue
    {
        [SerializeField] private SpellPowerType powerType;
        [SerializeField] private int maxCost;

        public override void Modify(Unit caster, Unit target, ref float value)
        {
            value *= (float)-caster.Attributes.ModifyPower(powerType, -maxCost) / maxCost;
        }
    }
}
