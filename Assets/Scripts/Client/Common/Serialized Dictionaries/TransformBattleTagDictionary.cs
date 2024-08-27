using Common;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TransformBattleTagDictionary : SerializedDictionary<TransformBattleTagDictionary.Entry, BattleHudTagType, RectTransform>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<BattleHudTagType, RectTransform>
        {
            [SerializeField] private BattleHudTagType tag;
            [SerializeField] private RectTransform rectTransform;

            public BattleHudTagType Key => tag;
            public RectTransform Value => rectTransform;
        }
    }
}