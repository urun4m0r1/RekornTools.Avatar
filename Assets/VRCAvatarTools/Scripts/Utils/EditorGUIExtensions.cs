using System;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public delegate void PropertyDrawerAction(Rect rect, SerializedProperty property, GUIContent label);

    public static class EditorGUIExtensions
    {
        [CanBeNull] public static T GetAttribute<T>(this SerializedProperty property) where T : Attribute
        {
            return PropertyUtility.GetAttribute<T>(property);

            T[] attributes = property.GetAttributes<T>();
            return attributes?.Length > 0 ? attributes[0] : null;
        }

        [ItemCanBeNull, CanBeNull]
        public static T[] GetAttributes<T>([CanBeNull] this SerializedProperty property) where T : Attribute
        {
            return PropertyUtility.GetAttributes<T>(property);

            if (property == null) return null;

            UnityEngine.Object target = property.serializedObject?.targetObject;
            if (!target) return null;

            string       path   = property.propertyPath;
            MemberInfo[] member = target.GetType().GetMember(path);
            if (member.Length == 0) return null;

            return member[0].GetCustomAttributes(typeof(T), false) as T[];
        }

        public static void OnGUIProperty(this SerializedProperty property,
            Rect rect,
            GUIContent label,
            [CanBeNull] PropertyDrawerAction action)
        {
            EditorGUI.BeginProperty(rect, label, property);
            EditorGUI.BeginChangeCheck();
            action?.Invoke(rect, property, label);
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }

        static readonly StringBuilder StringBuilder = new StringBuilder();

        public static readonly float SingleItemHeight =
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        [CanBeNull] public static SerializedProperty FindPropertyWithAutoPropertyName(
            [CanBeNull] this SerializedProperty obj,
            [NotNull] string propertyName) =>
            obj?.FindPropertyRelative(GetBackingFieldName(propertyName));

        [CanBeNull] public static SerializedProperty FindPropertyWithAutoPropertyName(
            [CanBeNull] this SerializedObject obj,
            [NotNull] string propertyName) =>
            obj?.FindProperty(GetBackingFieldName(propertyName));

        public static bool SimpleDisabledPropertyField([CanBeNull] this SerializedProperty obj,
            Rect rect,
            bool isDisabled = true)
        {
            EditorGUI.BeginDisabledGroup(isDisabled);
            bool result = obj.SimplePropertyField(rect);
            EditorGUI.EndDisabledGroup();
            return result;
        }

        public static bool SimplePropertyField([CanBeNull] this SerializedProperty obj,
            Rect rect)
        {
            if (obj == null)
            {
                EditorGUI.LabelField(rect, "ERROR:", "SerializedProperty is null");
                return false;
            }

            return EditorGUI.PropertyField(rect, obj, GUIContent.none, true);
        }

        public static float GetPropertyHeight([CanBeNull] this SerializedProperty obj) =>
            obj == null
                ? SingleItemHeight
                : EditorGUI.GetPropertyHeight(obj, true);

        [NotNull] public static string GetBackingFieldName([NotNull] this string propertyName)
        {
            StringBuilder.Length = 0;
            StringBuilder.Append("<");
            StringBuilder.Append(propertyName);
            StringBuilder.Append(">k__BackingField");
            return StringBuilder.ToString();
        }
    }
}
