using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace VRCAvatarTools
{
    public static class GameObjectExtensions
    {
        [CanBeNull]
        private static IEnumerable<GameObject> AllGameObjectsInProject =>
            Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        [CanBeNull] public static IEnumerable<GameObject> GetAllGameObjectsInScene =>
            AllGameObjectsInProject?.Where(IsEditableSceneObject);

        private static bool IsEditableSceneObject([CanBeNull] GameObject go)
        {
            if (!go) return false;

            var root        = go.transform.root;
            if (!root) root = go.transform;

            return !(root.hideFlags == HideFlags.NotEditable
                  || root.hideFlags == HideFlags.HideAndDontSave);
        }
    }
}
