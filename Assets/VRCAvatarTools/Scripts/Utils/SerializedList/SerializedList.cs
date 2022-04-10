using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace VRCAvatarTools
{
    public sealed class SerializedList : SerializedList<object>
    {
        [NotNull] public const string ListName = nameof(_items);
    }

    [Serializable]
    public class SerializedList<T> : IList<T>
    {
        [SerializeField, NotNull] protected List<T> _items = new List<T>();

#region Interface
        public IEnumerator<T>   GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_items).GetEnumerator();

        public void Add(T    item) => _items.Add(item);
        public bool Remove(T item) => _items.Remove(item);

        public bool Contains(T item) => _items.Contains(item);
        public int  IndexOf(T  item) => _items.IndexOf(item);

        public void Insert(int   index, T item) => _items.Insert(index, item);
        public void RemoveAt(int index) => _items.RemoveAt(index);

        public void CopyTo(T[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public void Clear() => _items.Clear();

        public int  Count      => _items.Count;
        public bool IsReadOnly => false;

        [CanBeNull] public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }
#endregion // Interface

        public void AddRange([CanBeNull] IEnumerable<T> target)
        {
            if (target == null) return;

            foreach (var item in target) Add(item);
        }

        public void RemoveRange([CanBeNull] IEnumerable<T> target)
        {
            if (target == null) return;

            foreach (var item in target) Remove(item);
        }

        public void Initialize([CanBeNull] IEnumerable<T> target)
        {
            Clear();
            AddRange(target);
        }
    }
}
