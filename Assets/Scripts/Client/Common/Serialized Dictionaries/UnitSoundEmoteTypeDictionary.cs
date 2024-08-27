using Common;
using Core;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class UnitSoundEmoteTypeDictionary : SerializedDictionary<UnitSoundEmoteTypeDictionary.Entry, EmoteType, UnitSounds>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<EmoteType, UnitSounds>
        {
            [SerializeField] private EmoteType emoteType;
            [SerializeField] private UnitSounds soundType;

            public EmoteType Key => emoteType;
            public UnitSounds Value => soundType;
        }
    }
}
