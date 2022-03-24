﻿using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(ShaderProperty))]
    public class ShaderPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, [NotNull] SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                SerializedProperty shader = property.FindPropertyWithAutoPropertyName(ShaderProperty.ShaderField);
                SerializedProperty index  = property.FindPropertyWithAutoPropertyName(ShaderProperty.IndexField);
                SerializedProperty name   = property.FindPropertyWithAutoPropertyName(ShaderProperty.NameField);
                SerializedProperty type   = property.FindPropertyWithAutoPropertyName(ShaderProperty.TypeField);
                SerializedProperty textureType =
                    property.FindPropertyWithAutoPropertyName(ShaderProperty.TextureTypeField);


                if (shader == null || index == null || name == null || type == null || textureType == null)
                {
                    EditorGUI.LabelField(position, label.text, "Invalid ShaderProperty");
                    return;
                }

                float w  = position.width;
                float w1 = w * 0.15f;
                float w2 = w * 0.1f;
                float w3 = w * 0.15f;
                float w4 = w * 0.4f;
                float w5 = w * 0.2f;

                position.y      += EditorGUIUtility.standardVerticalSpacing;
                position.height =  EditorGUIUtility.singleLineHeight;

                float padding = EditorGUIUtility.standardVerticalSpacing * 0.5f;

                EditorGUI.BeginDisabledGroup(true);
                {
                    position.width = w1 - padding;
                    EditorGUI.PropertyField(position, shader, GUIContent.none);
                    position.x += w1;

                    position.width = w2 - padding;
                    EditorGUI.PropertyField(position, index, GUIContent.none);
                    position.x += w2;

                    position.width = w3 - padding;
                    EditorGUI.PropertyField(position, type, GUIContent.none);
                    position.x += w3;

                    position.width = w4 - padding;
                    EditorGUI.PropertyField(position, name, GUIContent.none);
                    position.x += w4;
                }
                EditorGUI.EndDisabledGroup();

                position.width = w5;
                EditorGUI.BeginChangeCheck();
                {
                    textureType.enumValueIndex =
                        EditorGUI.Popup(position, textureType.enumValueIndex, textureType.enumDisplayNames);
                }
                EditorGUI.EndChangeCheck();
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUIUtilityExtensions.SingleItemHeight;
    }
}
