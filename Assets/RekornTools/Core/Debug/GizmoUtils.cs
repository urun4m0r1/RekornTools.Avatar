using System;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Debug
{
    public static class GizmoUtils
    {
        public static float ZoomLevel
        {
            get
            {
                if (SceneView.currentDrawingSceneView != null) return SceneView.currentDrawingSceneView.size;

                return 1;
            }
        }

        public static float InversedZoomLevel => 1f / ZoomLevel;

        public static float ClampedZoomLevel(float         farDistance) => Mathf.Clamp(ZoomLevel, 1f, farDistance);
        public static float ClampedInversedZoomLevel(float farDistance) => 1f / ClampedZoomLevel(farDistance);

        public static void DrawThickLine(Vector3 start, Vector3 end, float thickness, Color color) =>
            Handles.DrawBezier(start, end, start, end, color, null, thickness);
    }

    [Flags] public enum TransformTool
    {
        None   = 0,
        Move   = 1 << 0,
        Rotate = 1 << 1,
        Scale  = 1 << 2,
    }

    public enum DrawMode
    {
        Invisible,
        OnSelected,
        OnNonSelected,
        Always,
    }
}
