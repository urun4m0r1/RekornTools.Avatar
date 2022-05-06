using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    public static class ObjectExtensions
    {
#region Editor
        public static void DrawEditor([NotNull] this Object obj, bool isDisabled)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            {
                obj.DrawEditor();
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawEditor<T>([NotNull] this T obj) where T : Object
        {
            var editor = UnityEditor.Editor.CreateEditor(obj);
            if (editor != null) editor.OnInspectorGUI();
            EditorUtility.SetDirty(obj);
        }
#endregion // Editor
    }
}
