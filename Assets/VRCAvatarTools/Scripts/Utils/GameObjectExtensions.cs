using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public class GameObjectExtensions
    {
        public static IEnumerable<GameObject> GetAllGameObjectsInScene =>
            AllGameObjectsInProject.Where(IsEditableGameObjectInScene);

        static IEnumerable<GameObject> AllGameObjectsInProject =>
            (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));

        static bool IsEditableGameObjectInScene(GameObject go) =>
            !EditorUtility.IsPersistent(go.transform.root.gameObject) &&
            !(go.hideFlags == HideFlags.NotEditable ||
              go.hideFlags == HideFlags.HideAndDontSave);
    }
}
