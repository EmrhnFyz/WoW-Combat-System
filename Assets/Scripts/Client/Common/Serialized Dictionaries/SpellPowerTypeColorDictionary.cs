using Common;
using Core;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class SpellPowerTypeColorDictionary : SerializedDictionary<SpellPowerTypeColorDictionary.Entry, SpellPowerType, Color>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<SpellPowerType, Color>
        {
            [SerializeField] private SpellPowerType key;
            [SerializeField] private Color value;

            public SpellPowerType Key => key;
            public Color Value => value;
        }
    }
}
