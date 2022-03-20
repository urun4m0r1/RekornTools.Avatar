using System.Collections.Generic;
using UnityEngine;
using System;

namespace VRCAvatarTools
{
    public class SerializedKeyValuePair : SerializedKeyValuePair<object, object>
    {
        public static string KeyField   => nameof(Key);
        public static string ValueField => nameof(Value);
    }

    [Serializable]
    public class SerializedKeyValuePair<K, V>
    {
        [field: SerializeField] public K Key   { get; private set; }
        [field: SerializeField] public V Value { get; private set; }

        public SerializedKeyValuePair() { }

        public SerializedKeyValuePair(K key, V value)
        {
            Key   = key;
            Value = value;
        }

        public SerializedKeyValuePair(KeyValuePair<K, V> pair) : this(pair.Key, pair.Value) { }
    }
}
