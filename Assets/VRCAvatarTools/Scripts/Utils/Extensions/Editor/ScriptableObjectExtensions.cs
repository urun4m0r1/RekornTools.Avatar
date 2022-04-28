using JetBrains.Annotations;
using UnityEditor;

namespace VRCAvatarTools
{
    public static class ScriptableObjectExtensions
    {
#region Editor
        public static void DrawEditor([NotNull] this UnityEngine.Object obj, bool isDisabled)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            {
                obj.DrawEditor();
            }
            EditorGUI.EndDisabledGroup();
        }

        public static void DrawEditor<T>([NotNull] this T obj) where T : UnityEngine.Object
        {
            var editor = UnityEditor.Editor.CreateEditor(obj);
            if (editor != null) editor.OnInspectorGUI();
            EditorUtility.SetDirty(obj);
        }
#endregion // Editor
    }
}
