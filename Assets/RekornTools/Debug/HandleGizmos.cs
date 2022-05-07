#if UNITY_EDITOR
using UnityEditor;
#endif

using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Debug
{
    [DisallowMultipleComponent]
    public sealed class HandleGizmos : MonoBehaviour, IGizmos
    {
#if UNITY_EDITOR
        [field: SerializeField] public DrawMode      DrawMode { get; set; } = DrawMode.OnSelected;
        [SerializeField]               TransformTool tool = TransformTool.Move | TransformTool.Rotate;

        bool _isSelected;

        [UsedImplicitly]
        [CanEditMultipleObjects]
        [CustomEditor(typeof(HandleGizmos))]
        public sealed class Drawer : Editor
        {
            void OnEnable()
            {
                SceneView.duringSceneGui -= OnScene;
                SceneView.duringSceneGui += OnScene;
            }

            void OnScene([CanBeNull] SceneView sceneView)
            {
                var component = target as HandleGizmos;
                if (component == null) return;

                DrawGizmos(component);
            }

            [DrawGizmo(GizmoType.Selected)]
            static void OnSelected([NotNull] HandleGizmos t, GizmoType _) => t._isSelected = true;

            [DrawGizmo(GizmoType.NonSelected)]
            static void OnNonSelected([NotNull] HandleGizmos t, GizmoType _) => t._isSelected = false;

            static void DrawGizmos([NotNull] HandleGizmos target)
            {
                if (target.tool == TransformTool.None) return;

                if (target._isSelected)
                {
                    Tools.current = Tool.Custom;
                    if (target.WillDrawOnSelected()) DrawHandles(target);
                }
                else
                {
                    if (target.WillDrawOnNonSelected()) DrawHandles(target);
                }
            }

            static void DrawHandles([NotNull] HandleGizmos target)
            {
                DrawCustomHandles(target, out var position, out var rotation, out var scale);

                if (GUI.changed)
                {
                    Transform t = target.transform;

                    Undo.RecordObject(t, "Transform Change");
                    {
                        t.position      = position;
                        t.localRotation = rotation;
                        t.localScale    = scale;
                    }
                }
            }

            static void DrawCustomHandles([NotNull] HandleGizmos target, out Vector3 p, out Quaternion r, out Vector3 s)
            {
                var t = target.transform;
                p = t.position;
                r = t.localRotation;
                s = t.localScale;

                var flagMove   = target.tool.HasFlag(TransformTool.Move);
                var flagRotate = target.tool.HasFlag(TransformTool.Rotate);
                var flagScale  = target.tool.HasFlag(TransformTool.Scale);

                if (flagMove && flagRotate && flagScale)
                {
                    Handles.TransformHandle(ref p, ref r, ref s);
                }
                else
                {
                    if (flagScale) s  = Handles.ScaleHandle(s, p, r, HandleUtility.GetHandleSize(p) * 1.25f);
                    if (flagMove) p   = Handles.PositionHandle(p, r);
                    if (flagRotate) r = Handles.RotationHandle(r, p);
                }
            }
        }
#endif
    }
}
