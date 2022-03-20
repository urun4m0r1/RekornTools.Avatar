using System.Text;
using JetBrains.Annotations;
using UnityEditor;

namespace VRCAvatarTools
{
    public static class EditorGUIUtilityExtensions
    {
        static readonly StringBuilder StringBuilder = new StringBuilder();

        public static readonly float SingleItemHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public static SerializedProperty FindPropertyWithAutoPropertyName(
            [NotNull] this SerializedProperty obj, string propertyName) =>
            obj.FindPropertyRelative(GetBackingFieldName(propertyName));

        public static SerializedProperty FindPropertyWithAutoPropertyName(
            [NotNull] this SerializedObject obj, string propertyName) =>
            obj.FindProperty(GetBackingFieldName(propertyName));

        [NotNull]
        public static string GetBackingFieldName(string propertyName)
        {
            StringBuilder.Length = 0;
            StringBuilder.Append("<");
            StringBuilder.Append(propertyName);
            StringBuilder.Append(">k__BackingField");
            return StringBuilder.ToString();
        }
    }
}
