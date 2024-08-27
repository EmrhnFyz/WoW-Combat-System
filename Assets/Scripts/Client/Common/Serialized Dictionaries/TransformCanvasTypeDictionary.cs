using Common;
using System;
using UnityEngine;

namespace Client
{
    [Serializable]
    public class TransformCanvasTypeDictionary : SerializedDictionary<TransformCanvasTypeDictionary.Entry, InterfaceCanvasType, RectTransform>
    {
        [Serializable]
        public class Entry : ISerializedKeyValue<InterfaceCanvasType, RectTransform>
        {
            [SerializeField] private InterfaceCanvasType canvasType;
            [SerializeField] private RectTransform rectTransform;

            public InterfaceCanvasType Key => canvasType;
            public RectTransform Value => rectTransform;
        }
    }
}