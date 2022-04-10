using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public class SerializedKeyValueHelper
    {
        [CanBeNull] public SerializedProperty Key   { get; private set; }
        [CanBeNull] public SerializedProperty Value { get; private set; }

        [NotNull] private readonly string _keyName;
        [NotNull] private readonly string _valueName;

        [CanBeNull] private SerializedProperty _container;

        public SerializedKeyValueHelper([NotNull] string keyName, [NotNull] string valueName)
        {
            _keyName   = ReflectionExtensions.ResolveFieldName(keyName);
            _valueName = ReflectionExtensions.ResolveFieldName(valueName);
        }

        [NotNull] public SerializedKeyValueHelper Update([CanBeNull] SerializedProperty property)
        {
            if (_container == property) return this;

            _container = property;

            Key   = property?.ResolveProperty(_keyName);
            Value = property?.ResolveProperty(_valueName);

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
                Key.DisabledPropertyField(rect, keyDisabled);
            }

            rect.AppendHeight(ValueHeight);
            {
                Value.DisabledPropertyField(rect, valueDisabled);
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
                Key.DisabledPropertyField(rect, keyDisabled);
            }

            rect.AppendWidth(valueWidth);
            {
                Value.DisabledPropertyField(rect, valueDisabled);
            }
        }

        public float KeyHeight   => Key.GetHeight();
        public float ValueHeight => Value.GetHeight();

        public float MaxHeight
        {
            get
            {
                float keyHeight   = Key.GetHeight();
                float valueHeight = Value.GetHeight();

                return Mathf.Max(keyHeight, valueHeight);
            }
        }

        public float TotalHeight
        {
            get
            {
                float keyHeight   = Key.GetHeight();
                float valueHeight = Value.GetHeight();

                return keyHeight + valueHeight;
            }
        }
    }
}
