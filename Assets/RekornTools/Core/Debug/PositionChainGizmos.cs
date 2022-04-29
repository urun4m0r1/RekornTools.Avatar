using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Debug
{
    [DisallowMultipleComponent]
    public class PositionChainGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public   DrawMode        DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]                 Gradient        colorGradient  = new Gradient();
        [SerializeField, Range(0f, 30f)] float           lineThickness  = 5f;
        [SerializeField]                 bool            drawDirection  = true;
        [SerializeField]                 Color           directionColor = Color.white;
        [SerializeField]                 List<Transform> targets        = new List<Transform>();

        [UsedImplicitly] public class GizmosDrawer
        {
            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected([NotNull] PositionChainGizmos t, GizmoType _) => t.DrawOnSelected(DrawGizmos);

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected([NotNull] PositionChainGizmos t, GizmoType _) => t.DrawOnNonSelected(DrawGizmos);

            static void DrawGizmos([NotNull] PositionChainGizmos target)
            {
                var targets = target.targets;
                if (targets == null) return;
                if (targets.Count < 2) return;
                for (var i = 0; i < targets.Count - 1; i++)
                {
                    var t1 = targets[i];
                    var t2 = targets[i + 1];

                    if (t1 == null || t2 == null) continue;

                    var p1 = t1.position;
                    var p2 = t2.position;

                    if (target.colorGradient != null)
                    {
                        var color = target.colorGradient.Evaluate(i / (targets.Count - 1f));
                        GizmoUtils.DrawThickLine(p1, p2, target.lineThickness, color);
                    }

                    if (target.drawDirection) DrawArrow.ForGizmoTwoPoints(p1, p2, target.directionColor);
                }
            }
        }
#endif
    }
}
