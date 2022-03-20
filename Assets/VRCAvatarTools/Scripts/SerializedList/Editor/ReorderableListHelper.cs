using System.Collections.Generic;
using NaughtyAttributes.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VRCAvatarTools
{
    public class ReorderableListHelper
    {
        readonly Dictionary<string, ReorderableList> _cache = new Dictionary<string, ReorderableList>();

        readonly string _listName;
        readonly bool   _isMutable;
        readonly bool   _drawHeader;

        public ReorderableListHelper(string listName, bool isMutable = true, bool drawHeader = true)
        {
            _listName   = listName;
            _isMutable  = isMutable;
            _drawHeader = drawHeader;
        }

        public float GetHeight(SerializedProperty prop) => GetList(prop)?.GetHeight() ?? 0f;

        public void Draw(Rect rect, SerializedProperty prop) => GetList(prop)?.DoList(rect);

        ReorderableList GetList(SerializedProperty prop)
        {
            SerializedProperty listProperty = prop.FindPropertyRelative(_listName);

            if (listProperty == null) return null;

            return _cache.TryGetValue(listProperty.propertyPath,
                out ReorderableList cachedList)
                ? cachedList
                : CreateList(prop, listProperty);
        }

        ReorderableList CreateList(SerializedProperty prop, SerializedProperty listProperty)
        {
            var list = new ReorderableList(listProperty.serializedObject, listProperty,
                _isMutable, _drawHeader, _isMutable, _isMutable);

            if (!_drawHeader) list.headerHeight = 0f;

            list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, PropertyUtility.GetLabel(prop));

            list.drawElementCallback += (rect, index, _, __) =>
            {
                SerializedProperty elementProp = list.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, elementProp, GUIContent.none, true);
            };

            //TODO: 성능 문제 해결
            list.elementHeightCallback += index =>
            {
                SerializedProperty elementProp = list.serializedProperty.GetArrayElementAtIndex(index);
                return EditorGUI.GetPropertyHeight(elementProp);
            };

            _cache[listProperty.propertyPath] = list;

            return list;
        }
    }
}
