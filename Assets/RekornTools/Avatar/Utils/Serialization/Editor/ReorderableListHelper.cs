using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    /// <summary>
    /// Do not create static class of this, it will cause key collisions of cache dictionary.
    /// </summary>
    public class ReorderableListHelper
    {
        [NotNull] readonly Dictionary<string, ReorderableList> _cache = new Dictionary<string, ReorderableList>();

        [NotNull] readonly string _listName;

        [CanBeNull] SerializedProperty _container;
        [CanBeNull] SerializedProperty _listContainer;
        [CanBeNull] ReorderableList    _list;

        bool          IsReadOnly => _container.GetAttribute<ReadOnlyListAttribute>() != null || _isReadOnly;
        readonly bool _isReadOnly;

        bool          ItemNotSpan => _container.GetAttribute<ItemNotSpanAttribute>() != null || _itemNotSpan;
        readonly bool _itemNotSpan;

        public ReorderableListHelper([NotNull] string listName, bool isReadOnly = false, bool itemNotSpan = false)
        {
            _listName    = listName;
            _isReadOnly  = isReadOnly;
            _itemNotSpan = itemNotSpan;
        }

        [NotNull] public ReorderableListHelper Update([CanBeNull] SerializedProperty container)
        {
            if (_container != container)
            {
                _container = container;

                var listContainer = container?.ResolveProperty(_listName);
                if (_listContainer != listContainer)
                {
                    _listContainer = listContainer;

                    UpdateList();
                }
            }

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

        public float GetHeight()
        {
            try
            {
                return _list?.GetHeight() ?? EditorGUIExtensions.SingleItemHeight;
            }
            catch (NullReferenceException)
            {
                return EditorGUIExtensions.SingleItemHeight;
            }
        }

        void UpdateList()
        {
            if (_listContainer?.propertyPath == null) return;

            if (_cache.ContainsKey(_listContainer.propertyPath))
            {
                _list = _cache[_listContainer.propertyPath];
            }
            else
            {
                _list = CreateList(_listContainer, null, IsReadOnly, ItemNotSpan);
                _cache.Add(_listContainer.propertyPath, _list);
            }
        }

        [NotNull]
        static ReorderableList CreateList(
            [NotNull] SerializedProperty property, [CanBeNull] string header, bool isReadOnly, bool itemNotSpan)
        {
            var list = new ReorderableList(property.serializedObject, property)
            {
                draggable     = !isReadOnly,
                displayAdd    = !isReadOnly,
                displayRemove = !isReadOnly,
            };

            if (string.IsNullOrWhiteSpace(header)) list.headerHeight = 0f;

            list.drawHeaderCallback  += OnDrawHeader;
            list.drawElementCallback += OnDrawElement;

            if (itemNotSpan) list.elementHeightCallback += OnGetHeight;
            else list.elementHeight                     =  GetElementHeight();

            return list;

            void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
            {
                rect.RevertIndent(EditorGUI.indentLevel);
                GetElement(index).PropertyField(rect);
            }

            float OnGetHeight(int index) => GetElement(index).GetHeight();

            float GetElementHeight() =>
                property.arraySize > 0
                    ? OnGetHeight(0) + EditorGUIUtility.standardVerticalSpacing
                    : EditorGUIExtensions.SingleItemHeight;

            SerializedProperty GetElement(int index) => property.GetArrayElementAtIndex(index);

            void OnDrawHeader(Rect rect) => EditorGUI.LabelField(rect, header);
        }
    }
}
