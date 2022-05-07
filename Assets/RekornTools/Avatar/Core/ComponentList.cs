using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [System.Serializable]
    public class ComponentList<T> : SerializedList<T> where T : Component
    {
        void ShowDialog([NotNull] string message)
        {
            var header = $"[{nameof(SerializedList<T>)}<{typeof(T).Name}>]";
            Debug.LogWarning($"{header} {message}");
            EditorUtility.DisplayDialog(header, message, "Confirm");
        }

#region UnityObject
        public void DestroyItems()
        {
            var destroyTarget = new List<T>();
            var destroyFailed = new StringBuilder();

            foreach (var o in this)
            {
                if (o == null)
                {
                    // ReSharper disable once ExpressionIsAlwaysNull
                    destroyTarget.Add(o);
                }
                else if (!IsObjectPrefab(o))
                {
                    Undo.DestroyObjectImmediate(o.gameObject);
                    destroyTarget.Add(o);
                }
                else
                {
                    destroyFailed.Append($"{o.name}, ");
                }
            }

            if (destroyFailed.Length > 0)
            {
                var objectsList = destroyFailed.ToString().TrimEnd(',', ' ');
                ShowDialog($"Failed to destroy following objects: {objectsList}\n" +
                           "You might need to unpack prefabs before destroy them.");
            }

            RemoveRange(destroyTarget);
        }

        static bool IsObjectPrefab([NotNull] Object o) =>
            o && PrefabUtility.GetPrefabInstanceStatus(o) == PrefabInstanceStatus.Connected;

        public void Initialize([CanBeNull] Transform parent, [CanBeNull] string keyword = null)
        {
            var objects = parent == null
                ? GameObjectExtensions.GetAllGameObjectsInScene?.SelectMany(x => x == null ? null : x.GetComponents<T>())
                : parent.GetComponentsInChildren<T>();

            if (keyword != null && !string.IsNullOrWhiteSpace(keyword))
                objects = objects?.Where(x => x != null && x.name.Contains(keyword));

            Initialize(objects?.Where(x => x));
        }
#endregion // UnityObject

#region Selection
        public void SelectComponents()
        {
            if (TryGetSelections(out var selections)) Selection.objects = selections;
        }

        public bool TryGetSelections([NotNull] out Object[] selections)
        {
            selections = this.Select(x => x == null ? null : x.gameObject as Object).ToArray();
            return selections.Length != 0;
        }
#endregion // Selection
    }
}
