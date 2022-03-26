using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(CustomTextureList))]
    public class CustomTextureListDrawer : PropertyDrawer
    {
        readonly float _itemHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var typeProp     = property.FindPropertyRelative("Type");
            var shaderProp   = property.FindPropertyRelative("Shader");
            var propertyProp = property.FindPropertyRelative("Property");
            var listProp     = property.FindPropertyRelative("List");

            EditorGUI.PropertyField(position, typeProp);
            position.y += _itemHeight;

            shaderProp.objectReferenceValue = EditorGUI.ObjectField(position, "Shader", shaderProp.objectReferenceValue,
                typeof(Shader), false);
            position.y += _itemHeight;

            var shader = shaderProp.objectReferenceValue as Shader;
            if (shader)
            {
                var propertyCount = shader.GetPropertyCount();
                if (propertyCount > 0)
                {
                    var propertyNames = new List<string>();
                    for (var i = 0; i < propertyCount; i++)
                    {
                        if (shader.GetPropertyType(i) == ShaderPropertyType.Texture)
                        {
                            propertyNames.Add(shader.GetPropertyName(i));
                        }
                    }

                    var index = propertyNames.IndexOf(propertyProp.stringValue);
                    if (index < 0)
                    {
                        index = 0;
                    }

                    index                    =  EditorGUI.Popup(position, "Property", index, propertyNames.ToArray());
                    propertyProp.stringValue =  propertyNames[index];
                    position.y               += _itemHeight;
                }
            }

            EditorGUI.BeginChangeCheck();
            {
                EditorGUI.PropertyField(position, listProp, true);
            }
            if (EditorGUI.EndChangeCheck()) property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var listProp = property.FindPropertyRelative("List");

            return _itemHeight * 3 + EditorGUI.GetPropertyHeight(listProp, true);
        }
    }
}
