using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    [CustomPropertyDrawer(typeof(TexturePresetTable))]
    public class TexturePresetTableDrawer : SerializedKeyValueDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                EditorGUI.BeginChangeCheck();
                {
                    var w  = position.width;
                    var w1 = w * 0.3f;
                    var w2 = w * 0.7f;

                    var             typeName       = TexturePresetTable.TypeFieldName;
                    var             presetName     = TexturePresetTable.PresetFieldName;
                    var typeProperty   = property.ResolveProperty(typeName);
                    var presetProperty = property.ResolveProperty(presetName);

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
            EditorGUIExtensions.SingleItemHeight;
    }
}
