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

        public ReorderableListHelper(string listName) => _listName = listName;

        public string Header    { get; set; }
        public bool   IsMutable { get; set; } = true;
        public bool   IsSpan    { get; set; } = true;

        public float GetHeight(SerializedProperty prop) => GetList(prop)?.GetHeight() ?? 0f;

        public void Draw(Rect rect, SerializedProperty prop) => GetList(prop)?.DoList(rect);

        ReorderableList GetList(SerializedProperty prop)
        {
            SerializedProperty listProperty = prop.FindPropertyRelative(_listName);

            if (listProperty == null) return null;

            Header    = PropertyUtility.GetAttribute<ListHeaderAttribute>(prop)?.Header;
            IsMutable = PropertyUtility.GetAttribute<ListMutableAttribute>(prop)?.IsMutable == null;
            IsSpan    = PropertyUtility.GetAttribute<ListSpanAttribute>(prop)?.IsSpan       == null;

            return _cache.TryGetValue(listProperty.propertyPath,
                out ReorderableList cachedList)
                ? cachedList
                : CreateList(listProperty);
        }

        ReorderableList CreateList(SerializedProperty listProperty)
        {
            var list = new ReorderableList(listProperty.serializedObject, listProperty,
                IsMutable, true, IsMutable, IsMutable);

            if (string.IsNullOrWhiteSpace(Header)) list.headerHeight = 0f;

            list.drawHeaderCallback += rect => EditorGUI.LabelField(rect, Header);
            list.drawElementCallback += (rect, index, isActive, isFocused) =>
                EditorGUI.PropertyField(rect, listProperty.GetArrayElementAtIndex(index), GUIContent.none, true);

            if (!IsSpan)
            {
                list.elementHeightCallback += index =>
                {
                    SerializedProperty element = listProperty.GetArrayElementAtIndex(index);
                    return EditorGUI.GetPropertyHeight(element, true);
                };
            }
            else
            {
                if (listProperty.arraySize > 0)
                {
                    SerializedProperty element = listProperty.GetArrayElementAtIndex(0);
                    list.elementHeight = EditorGUI.GetPropertyHeight(element, true) +
                                         EditorGUIUtility.standardVerticalSpacing;
                }
                else
                {
                    list.elementHeight = EditorGUIUtilityExtensions.SingleItemHeight;
                }
            }

            _cache[listProperty.propertyPath] = list;

            return list;
        }
    }
}
