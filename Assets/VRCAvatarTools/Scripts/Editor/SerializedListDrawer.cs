using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : PropertyDrawer
    {
        readonly Dictionary<string, ReorderableList> _listsPerProp = new Dictionary<string, ReorderableList>();

        ReorderableList GetReorderableList([NotNull] SerializedProperty prop)
        {
            SerializedProperty listProperty = prop.FindPropertyRelative("list");

            if (listProperty == null) return null;

            if (_listsPerProp.TryGetValue(
                    listProperty.propertyPath,
                    out ReorderableList list))
                return list;

            list = new ReorderableList(
                listProperty.serializedObject,
                listProperty,
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true);
            _listsPerProp[listProperty.propertyPath] = list;

            list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, PropertyUtility.GetLabel(prop));

            list.drawElementCallback += (rect, index, isActive, isFocused) =>
            {
                SerializedProperty elementProp = list.serializedProperty.GetArrayElementAtIndex(index);

                if (elementProp.hasVisibleChildren)
                {
                    EditorGUI.PropertyField(rect, elementProp, includeChildren: true);
                }
                else
                {
                    EditorGUI.PropertyField(rect, elementProp, includeChildren: true, label: GUIContent.none);
                }
            };

            return list;
        }

        public override void OnGUI(Rect rect, [NotNull] SerializedProperty prop, GUIContent label) =>
            GetReorderableList(prop)?.DoList(rect);

        public override float GetPropertyHeight([NotNull] SerializedProperty prop, GUIContent label) =>
            GetReorderableList(prop)?.GetHeight() ?? base.GetPropertyHeight(prop, label);
    }
}
