using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    public class SerializedKeyValueHelper
    {
        [CanBeNull] public SerializedProperty Key   { get; private set; }
        [CanBeNull] public SerializedProperty Value { get; private set; }

        [NotNull] readonly string _keyName =
            ReflectionExtensions.ResolveFieldName(SerializedKeyValue.KeyFieldName);

        [NotNull] readonly string _valueName =
            ReflectionExtensions.ResolveFieldName(SerializedKeyValue.ValueFieldName);

        [CanBeNull] SerializedProperty _container;

        [NotNull] public SerializedKeyValueHelper Update([CanBeNull] SerializedProperty container)
        {
            if (_container != container)
            {
                _container = container;

                Key   = container?.ResolveProperty(_keyName);
                Value = container?.ResolveProperty(_valueName);
            }

            return this;
        }

        public float KeyHeight   => Key.GetHeight()   + EditorGUIUtility.standardVerticalSpacing;
        public float ValueHeight => Value.GetHeight() + EditorGUIUtility.standardVerticalSpacing;

        public float MaxHeight   => Mathf.Max(Key.GetHeight(), Value.GetHeight());
        public float TotalHeight => Key.GetHeight() + Value.GetHeight();

        public void DrawVertical(Rect rect, bool keyDisabled = false, bool valueDisabled = false)
        {
            rect.height = 0f;

            rect.AppendHeight(KeyHeight);
            Key.DisabledPropertyField(rect, keyDisabled);

            rect.AppendHeight(ValueHeight);
            Value.DisabledPropertyField(rect, valueDisabled);
        }

        public void DrawHorizontal(Rect rect, float keyWeight = 0.5f, bool keyDisabled = false, bool valueDisabled = false)
        {
            var keyWidth   = rect.width * keyWeight;
            var valueWidth = rect.width - keyWidth;
            rect.width = 0f;

            rect.AppendWidth(keyWidth);
            Key.DisabledPropertyField(rect, keyDisabled);

            rect.AppendWidth(valueWidth);
            Value.DisabledPropertyField(rect, valueDisabled);
        }
    }
}
