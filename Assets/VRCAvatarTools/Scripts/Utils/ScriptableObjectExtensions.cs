using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static UnityEngine.ScriptableObject;

namespace VRCAvatarTools
{
    public static class ScriptableObjectExtensions
    {
        [NotNull]
        public static T GetInstance<T>([CanBeNull] T obj) where T : ScriptableObject =>
            (obj == null ? CreateInstance<T>() : obj) ?? throw new InvalidOperationException();

        public static void DrawEditor([NotNull] this UnityEngine.Object obj) =>
            Editor.CreateEditor(obj)?.OnInspectorGUI();
    }
}
