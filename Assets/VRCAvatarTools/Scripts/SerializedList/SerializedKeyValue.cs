using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

namespace VRCAvatarTools
{
    public class SerializedKeyValue : SerializedKeyValue<object, object>
    {
        [NotNull] public static string KeyField   => nameof(Key);
        [NotNull] public static string ValueField => nameof(Value);
    }

    [Serializable]
    public abstract class SerializedKeyValue<K, V>
    {
        [field: SerializeField] [CanBeNull]                     public virtual K Key   { get; set; }
        [field: SerializeField, ListMutable(false)] [CanBeNull] public virtual V Value { get; set; }

        public SerializedKeyValue() { }

        public SerializedKeyValue(K key, V value)
        {
            Key   = key;
            Value = value;
        }

        public SerializedKeyValue(KeyValuePair<K, V> pair) : this(pair.Key, pair.Value) { }
    }
}
