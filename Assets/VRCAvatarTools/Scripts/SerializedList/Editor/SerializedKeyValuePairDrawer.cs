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
                if (!TryGetValue(property, out SerializedProperty key, out SerializedProperty value))
                {
                    EditorGUI.LabelField(position, label.text, "Invalid ShaderProperty");
                    return;
                }

                position.height = EditorGUI.GetPropertyHeight(key, true);
                EditorGUI.BeginDisabledGroup(true);
                {
                    EditorGUI.PropertyField(position, key, GUIContent.none, true);
                }
                EditorGUI.EndDisabledGroup();

                position.y      += position.height;

                position.height =  EditorGUI.GetPropertyHeight(value, true);
                EditorGUI.PropertyField(position, value, GUIContent.none, true);
            }
            EditorGUI.EndProperty();
        }

        static bool TryGetValue(SerializedProperty property, out SerializedProperty key, out SerializedProperty value)
        {
            key   = property.FindPropertyWithAutoPropertyName(SerializedKeyValuePair.KeyField);
            value = property.FindPropertyWithAutoPropertyName(SerializedKeyValuePair.ValueField);

            return key != null && value != null;
        }

        static float GetTotalHeight(SerializedProperty key, SerializedProperty value)
        {
            float keyHeight   = EditorGUI.GetPropertyHeight(key,   true);
            float valueHeight = EditorGUI.GetPropertyHeight(value, true);

            return keyHeight + valueHeight;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            TryGetValue(property, out SerializedProperty key, out SerializedProperty value)
                ? GetTotalHeight(key, value)
                : 0f;
    }
}
