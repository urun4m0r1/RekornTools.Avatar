using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRCAvatarTools
{
    [Serializable] public class SkinnedMeshRendererList : ComponentList<SkinnedMeshRenderer> { }

    [Serializable] public class TransformList : ComponentList<Transform> { }

    [Serializable]
    public class ComponentList<T> : IList<T> where T : Component
    {
        [SerializeField] protected List<T> list = new List<T>();

        const           string ClassName = nameof(ComponentList<T>);
        static readonly string TypeName  = typeof(T).Name;
        [NotNull]       string Header => $"[{ClassName}<{TypeName}>]";

        void Log(string        message) => Debug.Log($"{Header} {message}");
        void LogWarning(string message) => Debug.LogWarning($"{Header} {message}");
        void LogError(string   message) => Debug.LogError($"{Header} {message}");

        public void DestroyItems()
        {
            if (Count == 0)
            {
                LogWarning("Nothing to destroy.");
                return;
            }

            var toRemove             = new List<T>();
            var destroyFailedObjects = new StringBuilder();

            foreach (T o in this)
            {
                if (!o)
                {
                    toRemove.Add(o);
                }
                else if (!IsObjectPrefab(o))
                {
                    Undo.DestroyObjectImmediate(o.gameObject);
                    toRemove.Add(o);
                }
                else
                {
                    destroyFailedObjects.Append($"{o.name}, ");
                }
            }

            if (destroyFailedObjects.Length > 0)
            {
                string objectsList = destroyFailedObjects.ToString().TrimEnd(',', ' ');
                LogWarning($"Failed to destroy following objects: {objectsList}");
                Log("You might need to unpack prefabs before destroy them.");
            }

            RemoveRange(toRemove);
        }

        static bool IsObjectPrefab([NotNull] Object o)
        {
            if (!o) return false;

            return PrefabUtility.GetPrefabInstanceStatus(o) == PrefabInstanceStatus.Connected;
        }

        public void SelectComponents()
        {
            if (TryGetSelections(out Object[] selections)) Selection.objects = selections;
        }

        public bool TryGetSelections([NotNull] out Object[] selections)
        {
            selections = this.Where(x => x).Select(x => x.gameObject as Object).ToArray();

            bool isEmpty = selections.Length == 0;
            if (isEmpty) LogWarning("There are no items to select.");

            return !isEmpty;
        }

        public void InitializeFromName(Transform parent, [NotNull] string name = "")
        {
            if (!parent)
            {
                LogWarning("Parent is null.");
                return;
            }

            Clear();

            T[] objects = parent.GetComponentsInChildren<T>();
            foreach (T item in from o in objects
                     where o.name.Contains(name)
                     select o.GetComponent<T>()
                     into t
                     where t
                     select t)
            {
                Add(item);
            }

            if (Count == 0) LogWarning($"There are no meshes with name '{name}'.");
            else Log($"Found {Count} items.");
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
