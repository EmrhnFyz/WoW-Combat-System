using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "Unit Attribute Definition", menuName = "Game Data/Entities/Unit Attribute Definition", order = 1)]
    internal class UnitAttributeDefinition : ScriptableObject
    {
        [SerializeField] private int baseHealth;
        [SerializeField] private int baseMaxHealth;
        [SerializeField] private int baseSpellPower;
        [SerializeField] private int baseIntellect;
        [SerializeField] private float critPercentage;

        internal int BaseHealth => baseHealth;
        internal int BaseMaxHealth => baseMaxHealth;
        internal int BaseSpellPower => baseSpellPower;
        internal int BaseIntellect => baseIntellect;
        internal float CritPercentage => critPercentage;
    }
}
