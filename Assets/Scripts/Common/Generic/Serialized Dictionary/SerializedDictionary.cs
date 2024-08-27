﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    [Serializable]
    public abstract class SerializedDictionary<TItem, TKey, TValue> : IReadOnlySerializedDictionary<TKey, TValue> where TItem : ISerializedKeyValue<TKey, TValue>
    {
        [SerializeField] private List<TItem> items;
        [SerializeField] private TValue defaultValue;

        private readonly Dictionary<TKey, TValue> valuesByKey = new();

        public TValue DefaultValue => defaultValue;
        public IReadOnlyDictionary<TKey, TValue> ValuesByKey => valuesByKey;

        public void Register()
        {
            Unregister();

            foreach (TItem item in items)
            {
                valuesByKey.Add(item.Key, item.Value);
            }
        }

        public void Unregister()
        {
            valuesByKey.Clear();
        }

        public TValue Value(TKey key) => valuesByKey.TryGetValue(key, out TValue value) ? value : defaultValue;
    }
}
