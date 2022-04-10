using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [System.Serializable]
    public class ComponentList<T> : SerializedList<T> where T : Component
    {
        private void ShowDialog([NotNull] string message)
        {
            var header = $"[{nameof(SerializedList<T>)}<{typeof(T).Name}>]";
            Debug.LogWarning($"{header} {message}");
            EditorUtility.DisplayDialog(header, message, "Confirm");
        }

        public void DestroyItems()
        {
            var destroyTarget = new List<T>();
            var destroyFailed = new StringBuilder();

            foreach (var o in this)
            {
                if (!o)
                {
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

        private static bool IsObjectPrefab([NotNull] Object o) =>
            o && PrefabUtility.GetPrefabInstanceStatus(o) == PrefabInstanceStatus.Connected;

        public void SelectComponents()
        {
            if (TryGetSelections(out var selections)) Selection.objects = selections;
        }

        public bool TryGetSelections([NotNull] out Object[] selections)
        {
            selections = this.Select(x => x ? x.gameObject as Object : null).ToArray();
            return selections.Length != 0;
        }

        public void Initialize([CanBeNull] Transform parent, [NotNull] string keyword = "")
        {
            var objects = parent
                ? parent.GetComponentsInChildren<T>()
                : GameObjectExtensions.GetAllGameObjectsInScene?.SelectMany(x => x ? x.GetComponents<T>() : null);

            if (!string.IsNullOrWhiteSpace(keyword)) objects = objects?.Where(x => x && x.name.Contains(keyword));

            Initialize(objects?.Where(x => x).ToList() ?? new List<T>());
        }
    }
}
