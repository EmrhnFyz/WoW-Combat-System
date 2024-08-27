using Common;
using Core;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class ClassTypeSpriteDictionary : SerializedDictionary<ClassTypeSpriteDictionary.ClassIconSprite, ClassType, Sprite>
    {
        [Serializable]
        public class ClassIconSprite : ISerializedKeyValue<ClassType, Sprite>
        {
            [SerializeField] private ClassType classType;
            [SerializeField] private Sprite sprite;

            public ClassType Key => classType;
            public Sprite Value => sprite;
        }
    }
}
