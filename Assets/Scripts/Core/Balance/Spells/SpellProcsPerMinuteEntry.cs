using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class SpellProcsPerMinuteModifier
    {
        [SerializeField] private SpellProcsPerMinuteModType modType;
        [SerializeField] private int parameter;
        [SerializeField] private float modValue;

        public SpellProcsPerMinuteModType Type => modType;
        public int Parameter => parameter;
        public float Value => modValue;
    }
}