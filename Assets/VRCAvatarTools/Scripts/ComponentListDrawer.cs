using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SkinnedMeshRendererList))]
    [CustomPropertyDrawer(typeof(TransformList))]
    class ComponentListDrawer : PropertyDrawer
    {
        readonly GUIStyle _headerStyle = new GUIStyle(EditorStyles.foldoutHeader)
        {
            fontStyle = FontStyle.Normal,
        };

        readonly float _itemHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        float _height;

        public override void OnGUI(Rect position, [NotNull] SerializedProperty property, GUIContent label)
        {
            float initialHeight = position.y;

            EditorGUI.BeginProperty(position, label, property);

            BeginDrawHeader(ref position, property, label);

            if (property.isExpanded)
            {
                EditorGUI.indentLevel++;

                DrawButton(ref position, property, label, "DestroyItems");
                DrawButton(ref position, property, label, "SelectComponents");
                DrawArray(ref position, property.FindPropertyRelative("list"));

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndFoldoutHeaderGroup();

            EditorGUI.EndProperty();

            _height = position.y - initialHeight;
        }

        void BeginDrawHeader(ref Rect position, [NotNull] SerializedProperty property, GUIContent label)
        {
            position.height     = _itemHeight;
            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(position, property.isExpanded, label, _headerStyle);
            position.y          += position.height;
        }

        void DrawArray(ref Rect position, [NotNull] SerializedProperty array)
        {
            float height = _itemHeight;

            if (array.isExpanded) height += (array.arraySize + 1) * _itemHeight;

            var rect = new Rect(position.x, position.y, position.width, height);
            EditorGUI.PropertyField(rect, array, true);

            position.y += height;
        }

        void DrawButton(ref Rect position, [NotNull] SerializedProperty property, GUIContent label, string methodName)
        {
            var buttonRect = new Rect(position.x, position.y, position.width, _itemHeight);
            if (GUI.Button(buttonRect, methodName))
            {
                const BindingFlags flags = BindingFlags.Instance         |
                                           BindingFlags.Public           |
                                           BindingFlags.NonPublic        |
                                           BindingFlags.FlattenHierarchy |
                                           BindingFlags.InvokeMethod     |
                                           BindingFlags.IgnoreCase;

                Object       targetClass    = property.serializedObject.targetObject;
                PropertyInfo targetProperty = targetClass.GetType().GetProperty(label.text, flags);
                MethodInfo   destroyMethod  = targetProperty?.PropertyType.GetMethod(methodName, flags);

                Undo.RegisterCompleteObjectUndo(targetClass, methodName);
                destroyMethod?.Invoke(targetProperty.GetValue(targetClass), null);
            }

            position.y += _itemHeight;
        }

        public override float GetPropertyHeight([NotNull] SerializedProperty property, GUIContent label) => _height;
    }
}