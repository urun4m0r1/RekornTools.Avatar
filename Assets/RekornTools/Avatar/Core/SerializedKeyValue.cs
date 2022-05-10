using UnityEngine;
using System;
using JetBrains.Annotations;

namespace RekornTools.Avatar
{
    public sealed class SerializedKeyValue : SerializedKeyValue<object, object> { }

    [Serializable] public class HorizontalKeyValue<K, V> : SerializedKeyValue<K, V> { }

    [Serializable] public class SerializedKeyValue<K, V>
    {
        [field: SerializeField] [CanBeNull] public K Key   { get; set; }
        [field: SerializeField] [CanBeNull] public V Value { get; set; }

        protected SerializedKeyValue() { }

        protected SerializedKeyValue([CanBeNull] K key, [CanBeNull] V value)
        {
            Key   = key;
            Value = value;
        }
    }
}
