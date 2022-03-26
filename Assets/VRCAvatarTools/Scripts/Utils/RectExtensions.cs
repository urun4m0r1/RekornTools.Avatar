using UnityEngine;

namespace VRCAvatarTools
{
    public static class RectExtensions
    {
        public static void AppendWidth(this ref Rect rect, float width)
        {
            rect.x += rect.width;
            rect.width = width;
        }

        public static void AppendHeight(this ref Rect rect, float height)
        {
            rect.y += rect.height;
            rect.height = height;
        }
    }
}
