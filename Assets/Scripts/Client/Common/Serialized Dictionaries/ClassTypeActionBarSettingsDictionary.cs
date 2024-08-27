using Common;
using Core;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class ClassTypeActionBarSettingsDictionary : SerializedDictionary<ClassTypeActionBarSettingsDictionary.Entry, ClassType, List<ActionBarSettings>>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<ClassType, List<ActionBarSettings>>
        {
            [SerializeField] private ClassType key;
            [SerializeField] private List<ActionBarSettings> value;

            public ClassType Key => key;
            public List<ActionBarSettings> Value => value;
        }
    }
}
