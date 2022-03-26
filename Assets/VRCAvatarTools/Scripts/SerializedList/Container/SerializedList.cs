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
        [NotNull] public static readonly string ListName = nameof(list);
    }

    [Serializable]
    public class SerializedList<T> : IList<T>
    {
        [SerializeField, NotNull] protected List<T> list = new List<T>();

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

        public IEnumerator<T>   GetEnumerator() => list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)list).GetEnumerator();

        public void Add(T    item) => list.Add(item);
        public bool Remove(T item) => list.Remove(item);

        public bool Contains(T item) => list.Contains(item);
        public int  IndexOf(T  item) => list.IndexOf(item);

        public void Insert(int   index, T item) => list.Insert(index, item);
        public void RemoveAt(int index) => list.RemoveAt(index);

        public void CopyTo(T[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public void Clear() => list.Clear();

        public int  Count      => list.Count;
        public bool IsReadOnly => false;

        public T this[int index]
        {
            get => list[index];
            set => list[index] = value;
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
