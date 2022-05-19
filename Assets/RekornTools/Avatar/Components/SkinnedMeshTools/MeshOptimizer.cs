#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class MeshOptimizer : MonoBehaviour
    {
        [SerializeField] Transform            parent;
        [SerializeField] SkinnedMeshRenderers meshes = new SkinnedMeshRenderers();
        [SerializeField] Transform            anchorOverride;
        [SerializeField] Bounds               boundingBox = new Bounds(Vector3.zero, Vector3.one * 2);

        [SerializeField] [HideInInspector] Transform _prevParent;

        void Awake() => Refresh();

        void OnValidate()
        {
            if (_prevParent != parent) Refresh();
        }

        [Button]
        void Refresh()
        {
            _prevParent = parent;
            meshes?.Initialize(parent);
        }

        void OnDrawGizmosSelected()
        {
            if (meshes == null) return;
            foreach (var mesh in meshes)
            {
                if (mesh == null) continue;

                var prevBounds = mesh.localBounds;
                {
                    DrawBounds(mesh, Color.yellow);

                    mesh.localBounds = boundingBox;
                    RepaintRenderer(mesh);

                    DrawBounds(mesh, Color.green);
                }
                mesh.localBounds = prevBounds;
            }
        }

        static void DrawBounds([NotNull] Renderer renderer, Color color)
        {
            var rotation = renderer.transform.rotation;
            if (renderer is SkinnedMeshRenderer)
            {
                var transformRoot = renderer.transform.root;
                if (transformRoot != null)
                    rotation = transformRoot.rotation;
            }

            var bounds = renderer.bounds;
            var matrix = Matrix4x4.TRS(bounds.center, rotation, bounds.size);
            Gizmos.matrix = matrix;
            Gizmos.color  = color;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        static void RepaintRenderer([NotNull] Renderer renderer)
        {
            renderer.enabled = false;
            renderer.enabled = true;
        }

        [Button]
        void Optimize()
        {
            if (meshes == null) return;
            foreach (var mesh in meshes)
            {
                if (mesh == null) continue;
                mesh.UndoableAction(() =>
                {
                    mesh.probeAnchor               = anchorOverride;
                    mesh.localBounds               = boundingBox;
                    mesh.updateWhenOffscreen       = false;
                    mesh.skinnedMotionVectors      = false;
                    mesh.allowOcclusionWhenDynamic = true;
                });
            }
        }
    }
}
#endif
