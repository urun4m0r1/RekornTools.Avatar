﻿using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    public abstract class BasePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, [CanBeNull] SerializedProperty property, [CanBeNull] GUIContent label)
        {
            if (property == null) return;

            var indent = EditorGUI.indentLevel;
            EditorGUI.BeginProperty(rect, label, property);
            {
                EditorGUI.BeginChangeCheck();
                {
                    EditorGUI.indentLevel = 0;
                    rect.ApplyIndent(indent);
                    DrawProperty(rect, property, label, indent);
                }
                if (EditorGUI.EndChangeCheck()) property.serializedObject?.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
            EditorGUI.indentLevel = indent;
        }

        protected abstract void DrawProperty(Rect rect, [NotNull] SerializedProperty property, [CanBeNull] GUIContent label, int indent);

        public override float GetPropertyHeight([CanBeNull] SerializedProperty property, [CanBeNull] GUIContent _) => property.GetHeight();
    }
}
