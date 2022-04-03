using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace VRCAvatarTools
{
    public delegate void PropertyDrawerAction(Rect rect, SerializedProperty property, GUIContent label);

    public static class EditorGUIExtensions
    {
        public static readonly float SingleItemHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public static void OnGUIProperty(
            [CanBeNull] this SerializedProperty property,
            Rect                                rect,
            [CanBeNull] GUIContent              label,
            [CanBeNull] PropertyDrawerAction    action)
        {
            BeginProperty(rect, label, property);
            {
                BeginChangeCheck();
                {
                    action?.Invoke(rect, property, label);
                }
                if (EndChangeCheck()) property?.serializedObject?.ApplyModifiedProperties();
            }
            EndProperty();
        }

        public static bool DisabledPropertyField([CanBeNull] this SerializedProperty obj, Rect rect, bool isDisabled = true)
        {
            BeginDisabledGroup(isDisabled);
            bool result = obj.PropertyField(rect);
            EndDisabledGroup();
            return result;
        }

        public static bool PropertyField([CanBeNull] this SerializedProperty obj, Rect rect, string title = null)
        {
            if (obj == null)
            {
                LabelField(rect, "ERROR:", "SerializedProperty is null");
                return false;
            }

            return EditorGUI.PropertyField(rect, obj, title != null ? new GUIContent(title) : GUIContent.none, true);
        }

        public static float GetHeight([CanBeNull] this SerializedProperty obj) =>
            obj == null ? SingleItemHeight : GetPropertyHeight(obj, true);
    }
}
