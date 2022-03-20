using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedKeyValuePair<,>), true)]
    public class SerializedKeyValuePairDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                var key   = property.FindPropertyRelative("Key");
                var value = property.FindPropertyRelative("Value");

                var keyHeight   = EditorGUI.GetPropertyHeight(key,   label, true);
                var valueHeight = EditorGUI.GetPropertyHeight(value, label, true);

                var totalHeight = keyHeight + valueHeight;

                var keyRect   = new Rect(position.x, position.y,             position.width, keyHeight);
                var valueRect = new Rect(position.x, position.y + keyHeight, position.width, valueHeight);

                EditorGUI.PropertyField(keyRect,   key,   label);
                EditorGUI.PropertyField(valueRect, value, label);

                position.height = totalHeight;

                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var key   = property.FindPropertyRelative("Key");
            var value = property.FindPropertyRelative("Value");

            var keyHeight   = EditorGUI.GetPropertyHeight(key,   label, true);
            var valueHeight = EditorGUI.GetPropertyHeight(value, label, true);

            return keyHeight + valueHeight;
        }
    }
}
