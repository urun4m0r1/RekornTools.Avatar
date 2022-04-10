using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public abstract class SerializedPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, [CanBeNull] SerializedProperty property, [CanBeNull] GUIContent label)
        {
            if (property == null) return;

            EditorGUI.BeginProperty(rect, label, property);
            {
                EditorGUI.BeginChangeCheck();
                {
                    DrawProperty(rect, property, label);
                }
                if (EditorGUI.EndChangeCheck()) property.serializedObject?.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        protected abstract void DrawProperty(Rect rect, [NotNull] SerializedProperty property, [CanBeNull] GUIContent label);

        public override float GetPropertyHeight([CanBeNull] SerializedProperty property, [CanBeNull] GUIContent _) => property.GetHeight();
    }
}
