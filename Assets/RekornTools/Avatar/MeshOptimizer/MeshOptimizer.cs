#if UNITY_EDITOR
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public class MeshOptimizer : MonoBehaviour
    {
        [SerializeField] Transform            parent;
        [SerializeField] SkinnedMeshRenderers meshes = new SkinnedMeshRenderers();
        [SerializeField] Transform            anchorOverride;
        [SerializeField] Bounds               boundingBox;

        Transform _prevParent;

        void Awake()
        {
            meshes.Initialize(parent);
        }

        void OnValidate()
        {
            if (_prevParent != parent)
            {
                _prevParent = parent;
                meshes.Initialize(parent);
            }
        }

        void OnDrawGizmosSelected()
        {
            if (meshes       == null) return;
            if (meshes.Count == 0) return;

            foreach (var mesh in meshes)
            {
                if (mesh == null) continue;

                var prevBounds = mesh.localBounds;

                DrawBounds(mesh, Color.yellow);

                mesh.localBounds = boundingBox;
                RepaintRenderer(mesh);

                DrawBounds(mesh, Color.green);

                mesh.localBounds = prevBounds;
            }
        }

        static void DrawBounds([NotNull] Renderer renderer, Color color)
        {
            var bounds = renderer.bounds;

            var rotation = renderer.transform.rotation;

            if (renderer is SkinnedMeshRenderer)
            {
                var transformRoot = renderer.transform.root;
                if (transformRoot != null)
                    rotation = transformRoot.rotation;
            }

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
                Undo.RecordObject(mesh, nameof(MeshOptimizer));
                {
                    mesh.probeAnchor               = anchorOverride;
                    mesh.localBounds               = boundingBox;
                    mesh.updateWhenOffscreen       = false;
                    mesh.skinnedMotionVectors      = false;
                    mesh.allowOcclusionWhenDynamic = true;
                }
            }
        }
    }
}
#endif
