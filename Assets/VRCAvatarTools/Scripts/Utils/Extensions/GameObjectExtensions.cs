using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRCAvatarTools
{
    public static class GameObjectExtensions
    {
        [CanBeNull] private static IEnumerable<GameObject> AllGameObjectsInProject =>
            Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        [CanBeNull] public static IEnumerable<GameObject> GetAllGameObjectsInScene =>
            AllGameObjectsInProject?.Where(IsEditableSceneObject);

        private static bool IsEditableSceneObject([CanBeNull] GameObject go)
        {
            if (go == null) return false;

            var root               = go.transform.root;
            if (root == null) root = go.transform;

#if UNITY_EDITOR
            var isStoredOnDisk = EditorUtility.IsPersistent(root);
#else
            var isStoredOnDisk = false;
#endif
            return !isStoredOnDisk && !(root.hideFlags == HideFlags.NotEditable || root.hideFlags == HideFlags.HideAndDontSave);
        }
    }
}
