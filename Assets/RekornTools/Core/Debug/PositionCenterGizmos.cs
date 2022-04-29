using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Debug
{
    [DisallowMultipleComponent]
    public sealed class PositionCenterGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public   DrawMode        DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 Gradient        colorGradient  = new Gradient();
        [SerializeField, Range(0f, 30f)] float           lineThickness  = 5f;
        [SerializeField]                 bool            drawDirection  = true;
        [SerializeField]                 Color           directionColor = Color.white;
        [SerializeField]                 List<Transform> targets        = new List<Transform>();

        [UsedImplicitly] public sealed class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected([NotNull] PositionCenterGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected([NotNull] PositionCenterGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos([NotNull] PositionCenterGizmos target)
            {
                var targets = target.targets;
                if (targets == null) return;
                if (targets.Count < 1) return;

                var po = target.transform.position;
                for (var i = 0; i < targets.Count; i++)
                {
                    var t = targets[i];
                    if (t == null) continue;

                    var p = t.position;

                    if (target.colorGradient != null)
                    {
                        var color = target.colorGradient.Evaluate(i / (targets.Count - 1f));
                        GizmoUtils.DrawThickLine(po, p, target.lineThickness, color);
                    }

                    if (target.drawDirection) DrawArrow.ForGizmoTwoPoints(po, p, target.directionColor);
                }
            }
        }
#endif
    }
}
