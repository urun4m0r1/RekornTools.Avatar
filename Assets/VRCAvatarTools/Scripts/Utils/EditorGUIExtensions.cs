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

        public static bool SimpleDisabledPropertyField([CanBeNull] this SerializedProperty obj, Rect rect, bool isDisabled = true)
        {
            BeginDisabledGroup(isDisabled);
            bool result = obj.SimplePropertyField(rect);
            EndDisabledGroup();
            return result;
        }

        public static bool SimplePropertyField([CanBeNull] this SerializedProperty obj, Rect rect)
        {
            if (obj == null)
            {
                LabelField(rect, "ERROR:", "SerializedProperty is null");
                return false;
            }

            return PropertyField(rect, obj, GUIContent.none, true);
        }

        public static float GetHeight([CanBeNull] this SerializedProperty obj) =>
            obj == null ? SingleItemHeight : GetPropertyHeight(obj, true);
    }
}
