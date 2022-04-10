using UnityEditor;
using UnityEngine;
using static VRCAvatarTools.SerializedKeyValue;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedKeyValue<,>), true)]
    public class SerializedKeyValueDrawer : SerializedPropertyDrawer
    {
        static readonly SerializedKeyValueProperty KeyValueProperty = new SerializedKeyValueProperty(KeyField, ValueField);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            KeyValueProperty?.Update(property).DrawVertical(rect, true);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            KeyValueProperty?.Update(property).TotalHeight ?? 0f;
    }
}
