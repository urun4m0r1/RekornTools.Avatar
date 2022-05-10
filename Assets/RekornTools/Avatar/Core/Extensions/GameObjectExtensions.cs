using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RekornTools.Avatar
{
    public static class GameObjectExtensions
    {
        public static void UnpackPrefab([CanBeNull] this GameObject prefab)
        {
            if (PrefabUtility.GetPrefabInstanceStatus(prefab) == PrefabInstanceStatus.Connected)
                PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }

        [CanBeNull]
        public static IEnumerable<GameObject> AllGameObjectsInProject =>
            Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        [CanBeNull]
        public static IEnumerable<GameObject> GetAllGameObjectsInScene =>
            AllGameObjectsInProject?.Where(IsEditableSceneObject);

        static bool IsEditableSceneObject([CanBeNull] GameObject go)
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
