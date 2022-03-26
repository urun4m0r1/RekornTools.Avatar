using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedKeyValuePair<,>), true)]
    public class SerializedKeyValuePairDrawer : PropertyDrawer
    {
        static readonly string KeyName   = SerializedKeyValuePair.KeyField.GetBackingFieldName();
        static readonly string ValueName = SerializedKeyValuePair.ValueField.GetBackingFieldName();

        static readonly SerializedKeyValueProperty
            KeyValueProperty = new SerializedKeyValueProperty(KeyName, ValueName);

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                KeyValueProperty.Update(property).DrawVertical(position, true);
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            KeyValueProperty.Update(property).TotalHeight;
    }
}
