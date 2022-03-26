using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedKeyValue<,>), true)]
    public class SerializedKeyValueDrawer : PropertyDrawer
    {
        static readonly string KeyName   = SerializedKeyValue.KeyField.GetBackingFieldName();
        static readonly string ValueName = SerializedKeyValue.ValueField.GetBackingFieldName();

        static readonly SerializedKeyValueProperty
            KeyValueProperty = new SerializedKeyValueProperty(KeyName, ValueName);

        static readonly PropertyDrawerAction Draw = DrawProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) =>
            property.OnGUIProperty(position, label, Draw);

        static void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            KeyValueProperty.Update(property).DrawVertical(rect, true);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            KeyValueProperty.Update(property).TotalHeight;
    }
}
