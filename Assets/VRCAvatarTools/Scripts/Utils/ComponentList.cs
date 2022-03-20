using System;
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

    [Serializable] public class RendererList : ComponentList<Renderer> { }

    [Serializable] public class TransformList : ComponentList<Transform> { }

    [Serializable]
    public class ComponentList<T> : SerializedList<T> where T : Component
    {
        public void DestroyItems()
        {
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
                ShowDialog($"Failed to destroy following objects: {objectsList}\n" +
                           "You might need to unpack prefabs before destroy them.");
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
            return selections.Length != 0;
        }

        public void Initialize(Transform parent, [NotNull] string keyword = "")
        {
            Clear();

            T[] objects = { };

            if (parent)
            {
                objects = parent.GetComponentsInChildren<T>();
            }
            else
            {
                if (typeof(T) == typeof(Transform) && !EditorUtility.DisplayDialog(
                        "No parent selected",
                        "You didn't select a parent\n"                                 +
                        "Do you really want to search for all objects in the scene?\n" +
                        "This operation might takes a while.",
                        "Proceed",
                        "Cancel")) return;

                IEnumerable<GameObject> gameObjectsInScene = GameObjectExtensions.GetAllGameObjectsInScene;
                objects = gameObjectsInScene.SelectMany(x => x.GetComponents<T>()).ToArray();
            }


            foreach (T item in from o in objects
                     where o.name.Contains(keyword)
                     select o.GetComponent<T>()
                     into t
                     where t
                     select t)
            {
                Add(item);
            }
        }
    }
}
