using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRCAvatarTools
{
    [Serializable]
    public class SerializedKeyValuePair<K, V>
    {
        [SerializeField] public K Key;
        [SerializeField] public V Value;

        public SerializedKeyValuePair() { }

        public SerializedKeyValuePair(K key, V value)
        {
            Key   = key;
            Value = value;
        }

        public SerializedKeyValuePair(KeyValuePair<K, V> pair) : this(pair.Key, pair.Value) { }
    }
}
