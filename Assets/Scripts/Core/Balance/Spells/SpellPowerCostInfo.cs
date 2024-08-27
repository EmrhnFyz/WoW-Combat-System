using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class SpellPowerCostInfo
    {
        [SerializeField] private SpellPowerType spellPowerType;
        [SerializeField] private float powerCostPercentage;
        [SerializeField] private int powerCost;

        public SpellPowerType SpellPowerType => spellPowerType;
        public float PowerCostPercentage => powerCostPercentage;
        public int PowerCost => powerCost;
    }
}