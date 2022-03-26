using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(TexturePresetTable))]
    public class TexturePresetTableDrawer : SerializedKeyValuePairDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                EditorGUI.BeginChangeCheck();
                {
                    float w  = position.width;
                    float w1 = w * 0.3f;
                    float w2 = w * 0.7f;

                    string             typeName       = TexturePresetTable.TypeFieldName;
                    string             presetName     = TexturePresetTable.PresetFieldName;
                    SerializedProperty typeProperty   = property.FindPropertyWithAutoPropertyName(typeName);
                    SerializedProperty presetProperty = property.FindPropertyWithAutoPropertyName(presetName);

                    EditorGUI.BeginDisabledGroup(true);
                    {
                        position.width = w1;
                        EditorGUI.PropertyField(position, typeProperty, GUIContent.none, true);
                    }
                    EditorGUI.EndDisabledGroup();

                    position.x += position.width;

                    position.width = w2;
                    EditorGUI.PropertyField(position, presetProperty, GUIContent.none, true);
                }
                if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUIUtilityExtensions.SingleItemHeight;
    }
}
