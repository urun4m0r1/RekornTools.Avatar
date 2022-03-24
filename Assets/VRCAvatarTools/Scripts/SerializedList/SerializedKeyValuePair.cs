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
    public abstract class SerializedKeyValuePair<K, V>
    {
        [field: SerializeField]                     public virtual K Key   { get; set; }
        [field: SerializeField, ListMutable(false)] public virtual V Value { get; set; }

        public SerializedKeyValuePair() { }

        public SerializedKeyValuePair(K key, V value)
        {
            Key   = key;
            Value = value;
        }

        public SerializedKeyValuePair(KeyValuePair<K, V> pair) : this(pair.Key, pair.Value) { }
    }
}
