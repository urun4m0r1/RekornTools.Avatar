﻿using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VRCAvatarTools
{
    /// <summary>
    /// Do not create static class of this, it will cause key collisions of cache dictionary.
    /// </summary>
    public class ReorderableListHelper
    {
        [NotNull] private readonly Dictionary<string, ReorderableList> _cache = new Dictionary<string, ReorderableList>();

        [NotNull] private readonly string _listName;

        [CanBeNull] private SerializedProperty _container;
        [CanBeNull] private SerializedProperty _listContainer;
        [CanBeNull] private ReorderableList    _list;

        [CanBeNull] private string Header    => _container.GetAttribute<ListHeaderAttribute>()?.Header;
        private             bool   IsMutable => _container.GetAttribute<ListMutableAttribute>()?.IsMutable ?? ListMutableAttribute.Default;
        private             bool   IsSpan    => _container.GetAttribute<ListSpanAttribute>()?.IsSpan       ?? ListSpanAttribute.Default;

        public ReorderableListHelper([NotNull] string listName) => _listName = listName;

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

        public float GetHeight() => _list?.GetHeight() ?? EditorGUIExtensions.SingleItemHeight;

        private void UpdateList()
        {
            if (_listContainer?.propertyPath == null) return;

            if (_cache.ContainsKey(_listContainer.propertyPath))
            {
                _list = _cache[_listContainer.propertyPath];
            }
            else
            {
                _list = CreateList(_listContainer, Header, IsMutable, IsSpan);
                _cache.Add(_listContainer.propertyPath, _list);
            }
        }

        [NotNull] private static ReorderableList CreateList(
            [NotNull] SerializedProperty property, [CanBeNull] string header, bool isMutable, bool isSpan)
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
                GetElement(index).PropertyField(rect);
            }

            float OnGetHeight(int index) => GetElement(index).GetHeight();

            SerializedProperty GetElement(int index) => property.GetArrayElementAtIndex(index);

            float GetElementHeight() =>
                property.arraySize > 0
                    ? OnGetHeight(0) + EditorGUIUtility.standardVerticalSpacing
                    : EditorGUIExtensions.SingleItemHeight;

            void OnDrawHeader(Rect rect) => EditorGUI.LabelField(rect, header);
        }
    }
}