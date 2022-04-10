using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;

namespace VRCAvatarTools
{
    public sealed class SerializedKeyValue : SerializedKeyValue<object, object>
    {
        [NotNull] public static string KeyField   => nameof(Key);
        [NotNull] public static string ValueField => nameof(Value);
    }

    [Serializable] public class SerializedKeyValue<K, V>
    {
        [field: SerializeField] [CanBeNull]                     public K Key   { get; set; }
        [field: SerializeField, ListMutable(false)] [CanBeNull] public V Value { get; set; }

        protected SerializedKeyValue() { }

        protected SerializedKeyValue([CanBeNull] K key, [CanBeNull] V value)
        {
            Key   = key;
            Value = value;
        }

        protected SerializedKeyValue(KeyValuePair<K, V> pair) : this(pair.Key, pair.Value) { }
    }
}
