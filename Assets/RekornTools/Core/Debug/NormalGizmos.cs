using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Debug
{
    [DisallowMultipleComponent]
    public sealed class NormalGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public   DrawMode DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 bool     autoScaled    = true;
        [SerializeField, Range(0f, 10f)] float    lineLength    = 1f;
        [SerializeField, Range(0f, 30f)] float    lineThickness = 5f;

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected([NotNull] NormalGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected([NotNull] NormalGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos([NotNull] NormalGizmos target)
            {
                var t = target.transform;
                var   p = t.position;

                var scaledLength = GetScaledLength(target.lineLength);

                var px = p + t.right   * scaledLength;
                var py = p + t.up      * scaledLength;
                var pz = p + t.forward * scaledLength;

                GizmoUtils.DrawThickLine(p, px, target.lineThickness, Color.red);
                GizmoUtils.DrawThickLine(p, py, target.lineThickness, Color.green);
                GizmoUtils.DrawThickLine(p, pz, target.lineThickness, Color.blue);

                float GetScaledLength(float length)
                {
                    var result                   = length;
                    if (!target.autoScaled) result *= GizmoUtils.ZoomLevel;
                    return result;
                }
            }
        }
#endif
    }
}
