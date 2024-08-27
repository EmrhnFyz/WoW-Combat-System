using Common;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class SoundEntryCharacterSoundDictionary : SerializedDictionary<SoundEntryCharacterSoundDictionary.Entry, UnitSounds, SoundEntry>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<UnitSounds, SoundEntry>
        {
            [SerializeField] private UnitSounds soundType;
            [SerializeField] private SoundEntry soundEntry;

            public UnitSounds Key => soundType;
            public SoundEntry Value => soundEntry;
        }
    }
}