using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public static class GameObjectExtensions
    {
        [CanBeNull] public static IEnumerable<GameObject> GetAllGameObjectsInScene =>
            AllGameObjectsInProject?.Where(IsEditableSceneObject);

        [CanBeNull] static IEnumerable<GameObject> AllGameObjectsInProject =>
            Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        static bool IsEditableSceneObject([CanBeNull] GameObject go)
        {
            if (!go) return false;

            var root        = go.transform.root;
            if (!root) root = go.transform;

            return !EditorUtility.IsPersistent(root.gameObject) &&
                   !(go.hideFlags == HideFlags.NotEditable
                  || go.hideFlags == HideFlags.HideAndDontSave);
        }
    }
}
