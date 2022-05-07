using System;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;


#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

namespace RekornTools.Avatar
{
    public static class EditorExtensions
    {
        public static void ShowConfirmDialog<T>([NotNull] this T script, [NotNull] string message) where T : MonoBehaviour
        {
            var header = $"[{typeof(T)}({script.gameObject.name})]";
            Debug.LogWarning($"{header} {message}");
#if UNITY_EDITOR
            EditorUtility.DisplayDialog(header, message, "Confirm");
#endif // UNITY_EDITOR
        }

        public static void UndoableAction([NotNull] this Object target, [NotNull] Action action) =>
            UndoableAction(target, target.name, action);

        public static void UndoableAction([NotNull] this Object target, [NotNull] string actionName, [NotNull] Action action)
        {
#if UNITY_EDITOR
            Undo.RegisterCompleteObjectUndo(target, actionName);
            action();
            Undo.FlushUndoRecordObjects();
#endif // UNITY_EDITOR
        }
    }
}
