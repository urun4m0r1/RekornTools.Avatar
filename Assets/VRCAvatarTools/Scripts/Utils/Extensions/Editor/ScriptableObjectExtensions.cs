using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ScriptableObject;

namespace VRCAvatarTools
{
    public static class ScriptableObjectExtensions
    {
        [NotNull] public static T GetInstance<T>([CanBeNull] ref T obj) where T : ScriptableObject
        {
            obj = obj.GetInstance();
            return obj;
        }

        [NotNull] public static T GetInstance<T>([CanBeNull] this T obj) where T : ScriptableObject =>
            (obj == null ? CreateInstance<T>() : obj) ?? throw new InvalidOperationException();

        public static void DrawEditor([NotNull] this UnityEngine.Object obj, bool isDisabled)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            {
                obj.DrawEditor();
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawEditor([NotNull] this UnityEngine.Object obj)
        {
            var editor = UnityEditor.Editor.CreateEditor(obj);
            if (editor) editor.OnInspectorGUI();
            EditorUtility.SetDirty(obj);
        }
    }
}
