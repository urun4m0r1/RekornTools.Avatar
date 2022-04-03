using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public class SerializedList : SerializedList<object>
    {
        [NotNull] public static readonly string ListName = nameof(_items);
    }

    [Serializable]
    public class SerializedList<T> : IList<T>
    {
        [SerializeField, NotNull] protected List<T> _items = new List<T>();

        static readonly string ClassName = nameof(SerializedList<T>);
        static readonly string TypeName  = typeof(T).Name;
        [NotNull]       string Header => $"[{ClassName}<{TypeName}>]";

        protected void ShowDialog(string message)
        {
            Debug.LogWarning($"{Header} {message}");
            EditorUtility.DisplayDialog(
                Header,
                message,
                "Confirm");
        }

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

        public T this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public void AddRange([NotNull] List<T> target)
        {
            foreach (T item in target) Add(item);
        }

        public void RemoveRange([NotNull] List<T> target)
        {
            foreach (T item in target) Remove(item);
        }

        public void Initialize([NotNull] List<T> target)
        {
            Clear();
            AddRange(target);
        }

        #endregion
    }
}
