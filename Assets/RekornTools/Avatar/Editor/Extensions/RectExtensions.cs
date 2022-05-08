using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    public static class RectExtensions
    {
        public static readonly float IndentWidth = 15f;

        public static void ApplyIndent(this ref Rect rect, int indent)
        {
            rect.x     += indent * IndentWidth;
            rect.width -= indent * IndentWidth;
        }

        public static void RevertIndent(this ref Rect rect, int indent)
        {
            rect.x     -= indent * IndentWidth;
            rect.width += indent * IndentWidth;
        }

        public static void AppendWidth(this ref Rect rect, float width)
        {
            rect.x     += rect.width;
            rect.width =  width;
        }

        public static void AppendHeight(this ref Rect rect, float height)
        {
            rect.y      += rect.height;
            rect.height =  height;
        }

        public static void AppendHeight(this ref Rect rect, [CanBeNull] SerializedProperty property)
        {
            rect.AppendHeight(property.GetHeight());
            rect.y += EditorGUIUtility.standardVerticalSpacing;
        }
    }
}
