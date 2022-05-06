using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    public sealed class SerializedDictionary : SerializedDictionary<SerializedKeyValue, object, object>
    {
        [NotNull] public const string FieldName = nameof(Items);
    }

    [Serializable]
    public class SerializedDictionary<TKeyValue, TKey, TValue> :
        Dictionary<TKey, TValue>, ISerializationCallbackReceiver
        where TKeyValue : SerializedKeyValue<TKey, TValue>, new()
    {
        [SerializeField] [NotNull] protected List<TKeyValue> Items = new List<TKeyValue>();

        public void OnBeforeSerialize()
        {
            Items.Clear();
            foreach (var item in this)
            {
                var pair = new TKeyValue
                {
                    Key   = item.Key,
                    Value = item.Value,
                };

                Items.Add(pair);
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var item in Items)
            {
                Add(item.Key, item.Value);
            }
        }
    }
}
