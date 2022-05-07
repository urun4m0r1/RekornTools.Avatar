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
            if (meshes == null) return;
            if (meshes.Count == 0) return;

            foreach (var mesh in meshes)
            {
                if (mesh == null) continue;

                var prevBounds = mesh.localBounds;
                var bounds     = mesh.bounds;

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(bounds.center, bounds.size);

                mesh.localBounds = boundingBox;
                RepaintRenderer(mesh);

                bounds       = mesh.bounds;
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(bounds.center, bounds.size);

                mesh.localBounds = prevBounds;
            }
        }

        [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
        static void RepaintRenderer<T>([NotNull] T renderer) where T : Renderer
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
