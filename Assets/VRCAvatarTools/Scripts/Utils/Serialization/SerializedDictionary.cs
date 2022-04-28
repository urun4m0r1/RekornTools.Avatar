using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace VRCAvatarTools
{
    public sealed class SerializedDictionary : SerializedDictionary<SerializedKeyValue, object, object>
    {
        [NotNull] public const string FieldName = nameof(_items);
    }

    [Serializable]
    public class SerializedDictionary<TKeyValue, TKey, TValue> :
        Dictionary<TKey, TValue>, ISerializationCallbackReceiver
        where TKeyValue : SerializedKeyValue<TKey, TValue>, new()
    {
        [SerializeField] [NotNull] protected List<TKeyValue> _items = new List<TKeyValue>();

        public void OnBeforeSerialize()
        {
            _items.Clear();
            foreach (var item in this)
            {
                var pair = new TKeyValue
                {
                    Key   = item.Key,
                    Value = item.Value,
                };

                _items.Add(pair);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var item in _items)
            {
                Add(item.Key, item.Value);
            }
        }
    }
}
