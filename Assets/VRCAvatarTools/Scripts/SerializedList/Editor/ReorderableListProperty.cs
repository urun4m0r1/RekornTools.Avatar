using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VRCAvatarTools
{
    public class ReorderableListProperty
    {
        [NotNull] readonly string _listName;

        [CanBeNull] SerializedProperty _container;
        [CanBeNull] ReorderableList    _list;

        public ReorderableListProperty([NotNull] string listName) => _listName = listName;

        [NotNull] public ReorderableListProperty Update([CanBeNull] SerializedProperty property)
        {
            if (_container == property) return this;

            _container = property;

            string header = _container.GetAttribute<ListHeaderAttribute>()?.Header;
            bool isMutable = _container.GetAttribute<ListMutableAttribute>()?.IsMutable ?? ListMutableAttribute.Default;
            bool isSpan = _container.GetAttribute<ListSpanAttribute>()?.IsSpan ?? ListSpanAttribute.Default;

            SerializedProperty listProperty = _container?.FindPropertyRelative(_listName);
            _list = CreateList(listProperty, header, isMutable, isSpan);

            return this;
        }

        public void Draw(Rect rect)
        {
            if (_list == null)
            {
                EditorGUI.LabelField(rect, "ERROR:", "SerializedProperty is null");
                return;
            }

            _list.DoList(rect);
        }

        public float GetHeight() => _list?.GetHeight() ?? EditorGUIExtensions.SingleItemHeight;

        [CanBeNull]
        static ReorderableList CreateList([CanBeNull] SerializedProperty property,
            [CanBeNull] string header,
            bool isMutable,
            bool isSpan)
        {
            if (property == null) return null;

            var list = new ReorderableList(property.serializedObject, property,
                isMutable, true, isMutable, isMutable);

            if (string.IsNullOrWhiteSpace(header)) list.headerHeight = 0f;

            list.drawHeaderCallback  += OnDrawHeader;
            list.drawElementCallback += OnDrawElement;

            if (isSpan) list.elementHeight  =  GetElementHeight();
            else list.elementHeightCallback += OnGetHeight;

            return list;

            void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused) =>
                GetElement(index).SimplePropertyField(rect);

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
