#if UNITY_EDITOR
using RekornTools.Math;
using UnityEditor;
#endif

using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Debug
{
    [DisallowMultipleComponent]
    public sealed class NameTagGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]               string   nameOverride = string.Empty;

        [Header("Font Settings")]
        [SerializeField] bool autoScaled = true;

        [SerializeField]                  Color textColor                = Color.white;
        [SerializeField, Range(0f, 100f)] int   fontSize                 = 80;
        [SerializeField, Range(1f, 10f)]  float fontSizeClippingDistance = 5f;

        [Header("Position")]
        [SerializeField] bool distanceAutoScaled = true;

        [SerializeField] Color                       lineColor = Color.magenta;
        [SerializeField] VectorExtensions.NormalAxis axis      = VectorExtensions.NormalAxis.Up;

        [SerializeField, Range(0f, 10f)] float distance = 1.5f;

        readonly GUIStyle _style = new GUIStyle();

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected([NotNull] NameTagGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected([NotNull] NameTagGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos([NotNull] NameTagGizmos target)
            {
                if (target._style != null)
                {
                    target._style.fontSize = GetScaledFontSize(target.fontSize);
                    if (target._style.normal != null) target._style.normal.textColor = target.textColor;
                }

                var tagName = string.IsNullOrWhiteSpace(target.nameOverride)
                    ? target.gameObject.name
                    : target.nameOverride;

                var t  = target.transform;
                var p  = t.position;
                var tp = p + VectorExtensions.GetNormal(target.axis) * GetScaledLength(target.distance);

                Handles.Label(tp, tagName, target._style);

                Gizmos.color = target.lineColor;
                Gizmos.DrawLine(p, tp);

                int GetScaledFontSize(int size)
                {
                    var result = size;
                    if (target.autoScaled)
                        result = (int)(result * GizmoUtils.ClampedInversedZoomLevel(target.fontSizeClippingDistance));
                    return result;
                }

                float GetScaledLength(float length)
                {
                    var result                             = length;
                    if (!target.distanceAutoScaled) result *= GizmoUtils.ZoomLevel;
                    return result;
                }
            }
        }
#endif
    }
}
