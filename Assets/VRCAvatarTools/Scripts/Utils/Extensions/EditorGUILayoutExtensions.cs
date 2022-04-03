using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace VRCAvatarTools
{
    public static class EditorGUILayoutExtensions
    {
#region Decorator
        public static void HorizontalLine() => LabelField("", GUI.skin.horizontalSlider);
#endregion

#region Extensions
        [CanBeNull] public static T ObjectField<T>(
            [CanBeNull] string label, [CanBeNull] T obj, bool allowSceneObjects)
            where T : UnityEngine.Object =>
            (T)EditorGUILayout.ObjectField(label, obj, typeof(T), allowSceneObjects);

        [CanBeNull] public static T EnumDropdown<T>(
            [CanBeNull] string label, [NotNull] T selected)
            where T : Enum
        {
            var e = EnumPopup(label, selected);
            return e != null ? (T)e : selected;
        }
#endregion
    }
}
