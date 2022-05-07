using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    public sealed class SerializedList : SerializedList<object>
    {
        [NotNull] public const string FieldName = nameof(Items);
    }

    [Serializable]
    public class SerializedList<T> : IList<T>
    {
        [SerializeField] [NotNull] protected List<T> Items = new List<T>();

#region Interface
        public IEnumerator<T>   GetEnumerator() => Items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Items).GetEnumerator();

        public void Add(T    item) => Items.Add(item);
        public bool Remove(T item) => Items.Remove(item);

        public bool Contains(T item) => Items.Contains(item);
        public int  IndexOf(T  item) => Items.IndexOf(item);

        public void Insert(int   index, T item) => Items.Insert(index, item);
        public void RemoveAt(int index) => Items.RemoveAt(index);

        public void CopyTo(T[] array, int arrayIndex) => Items.CopyTo(array, arrayIndex);
        public void Clear() => Items.Clear();

        public int  Count      => Items.Count;
        public bool IsReadOnly => false;

        [CanBeNull] public T this[int index]
        {
            get => Items[index];
            set => Items[index] = value;
        }
#endregion // Interface

#region Extensions
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
#endregion // Extensions
    }
}
