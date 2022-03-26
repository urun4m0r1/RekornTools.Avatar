using System.Text;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public class SerializedKeyValueProperty
    {
        public SerializedProperty Key   { get; private set; }
        public SerializedProperty Value { get; private set; }

        public readonly string KeyName;
        public readonly string ValueName;

        SerializedProperty _container;

        public SerializedKeyValueProperty(string keyName, string valueName)
        {
            KeyName   = keyName;
            ValueName = valueName;
        }

        [NotNull] public SerializedKeyValueProperty Update([CanBeNull] SerializedProperty property)
        {
            if (_container == property) return this;

            Key        = property?.FindPropertyRelative(KeyName);
            Value      = property?.FindPropertyRelative(ValueName);
            _container = property;

            return this;
        }

        public void DrawVertical(
            Rect rect,
            bool keyDisabled = false,
            bool valueDisabled = false)
        {
            rect.height = 0f;

            rect.AppendHeight(KeyHeight);
            {
                Key.SimpleDisabledPropertyField(rect, keyDisabled);
            }

            rect.AppendHeight(ValueHeight);
            {
                Value.SimpleDisabledPropertyField(rect, valueDisabled);
            }
        }

        public void DrawHorizontal(
            Rect rect,
            float keyWeight = 0.5f,
            bool keyDisabled = false,
            bool valueDisabled = false)
        {
            float keyWidth   = rect.width * keyWeight;
            float valueWidth = rect.width - keyWidth;
            rect.width = 0f;

            rect.AppendWidth(keyWidth);
            {
                Key.SimpleDisabledPropertyField(rect, keyDisabled);
            }

            rect.AppendWidth(valueWidth);
            {
                Value.SimpleDisabledPropertyField(rect, valueDisabled);
            }
        }

        public float KeyHeight   => Key.GetPropertyHeight();
        public float ValueHeight => Value.GetPropertyHeight();

        public float MaxHeight
        {
            get
            {
                float keyHeight   = Key.GetPropertyHeight();
                float valueHeight = Value.GetPropertyHeight();

                return Mathf.Max(keyHeight, valueHeight);
            }
        }

        public float TotalHeight
        {
            get
            {
                float keyHeight   = Key.GetPropertyHeight();
                float valueHeight = Value.GetPropertyHeight();

                return keyHeight + valueHeight;
            }
        }
    }

    public static class EditorGUIUtilityExtensions
    {
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

        [NotNull]
        public static string GetBackingFieldName([NotNull] this string propertyName)
        {
            StringBuilder.Length = 0;
            StringBuilder.Append("<");
            StringBuilder.Append(propertyName);
            StringBuilder.Append(">k__BackingField");
            return StringBuilder.ToString();
        }
    }
}
