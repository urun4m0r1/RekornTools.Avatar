using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VRCAvatarTools
{
    public class ReorderableListProperty
    {
        // Do not make this static, it will cause key collisions of cache dictionary.
        readonly Dictionary<string, ReorderableList> _cache = new Dictionary<string, ReorderableList>();

        [NotNull] readonly string _listName;

        [CanBeNull] SerializedProperty _container;
        [CanBeNull] SerializedProperty _listContainer;
        [CanBeNull] ReorderableList    _list;

        [CanBeNull] string Header => _container.GetAttribute<ListHeaderAttribute>()?.Header;
        bool IsMutable => _container.GetAttribute<ListMutableAttribute>()?.IsMutable ?? ListMutableAttribute.Default;
        bool IsSpan => _container.GetAttribute<ListSpanAttribute>()?.IsSpan ?? ListSpanAttribute.Default;

        public ReorderableListProperty([NotNull] string listName) => _listName = listName;

        [NotNull] public ReorderableListProperty Update([CanBeNull] SerializedProperty property)
        {
            if (_container == property) return this;

            _container = property;

            SerializedProperty listProperty = property?.FindPropertyRelative(_listName);
            if (_listContainer == listProperty) return this;

            _listContainer = listProperty;

            UpdateList();

            return this;
        }

        public void Draw(Rect rect)
        {
            rect.ApplyIndent(EditorGUI.indentLevel);
            if (_list == null)
            {
                EditorGUI.LabelField(rect, "ERROR:", "SerializedProperty is null");
                return;
            }

            _list.DoList(rect);
        }

        public float GetHeight() => _list?.GetHeight() ?? EditorGUIExtensions.SingleItemHeight;

        void UpdateList()
        {
            if (_listContainer?.propertyPath == null) return;

            if (!_cache.ContainsKey(_listContainer.propertyPath))
            {
                _list = CreateList(_listContainer, Header, IsMutable, IsSpan);
                _cache.Add(_listContainer.propertyPath, _list);
            }
            else
            {
                _list = _cache[_listContainer.propertyPath];
            }
        }

        [NotNull] static ReorderableList CreateList([NotNull] SerializedProperty property,
            [CanBeNull] string header,
            bool isMutable,
            bool isSpan)
        {
            var list = new ReorderableList(property.serializedObject, property)
            {
                draggable     = isMutable,
                displayAdd    = isMutable,
                displayRemove = isMutable,
            };

            if (string.IsNullOrWhiteSpace(header)) list.headerHeight = 0f;

            list.drawHeaderCallback  += OnDrawHeader;
            list.drawElementCallback += OnDrawElement;

            if (isSpan) list.elementHeight  =  GetElementHeight();
            else list.elementHeightCallback += OnGetHeight;

            return list;

            void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                rect.RevertIndent(EditorGUI.indentLevel);
                GetElement(index).SimplePropertyField(rect);
            }

            float OnGetHeight(int index) => GetElement(index).GetPropertyHeight();

            SerializedProperty GetElement(int index) => property.GetArrayElementAtIndex(index);

            float GetElementHeight() =>
                property.arraySize > 0
                    ? OnGetHeight(0) + EditorGUIUtility.standardVerticalSpacing
                    : EditorGUIExtensions.SingleItemHeight;

            void OnDrawHeader(Rect rect) => EditorGUI.LabelField(rect, header);
        }
    }
}
