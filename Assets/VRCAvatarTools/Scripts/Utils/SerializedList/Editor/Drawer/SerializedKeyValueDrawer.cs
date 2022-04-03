using UnityEditor;
using UnityEngine;
using static VRCAvatarTools.SerializedKeyValue;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedKeyValue<,>), true)]
    public class SerializedKeyValueDrawer : PropertyDrawer
    {
        static readonly SerializedKeyValueProperty KeyValueProperty = new SerializedKeyValueProperty(KeyField, ValueField);

        static readonly PropertyDrawerAction Draw = DrawProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) =>
            property.OnGUIProperty(position, label, Draw);

        static void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            KeyValueProperty.Update(property).DrawVertical(rect, true);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            KeyValueProperty.Update(property).TotalHeight;
    }
}
