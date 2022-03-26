using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public class SerializedKeyValueProperty
    {
        [CanBeNull] public SerializedProperty Key   { get; private set; }
        [CanBeNull] public SerializedProperty Value { get; private set; }

        [NotNull] readonly string _keyName;
        [NotNull] readonly string _valueName;

        [CanBeNull] SerializedProperty _container;

        public SerializedKeyValueProperty([NotNull] string keyName, [NotNull] string valueName)
        {
            _keyName   = keyName;
            _valueName = valueName;
        }

        [NotNull] public SerializedKeyValueProperty Update([CanBeNull] SerializedProperty property)
        {
            if (_container == property) return this;

            _container = property;

            Key   = property?.FindPropertyRelative(_keyName);
            Value = property?.FindPropertyRelative(_valueName);

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
}
