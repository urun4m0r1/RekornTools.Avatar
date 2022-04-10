#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public class MeshOptimizer : MonoBehaviour
    {
        [SerializeField] private Transform               parent;
        [SerializeField] private SkinnedMeshRenderers meshes = new SkinnedMeshRenderers();
        [SerializeField] private Transform               anchorOverride;
        [SerializeField] private Bounds                  boundingBox;

        private Transform _prevParent;

        private void Awake()
        {
            meshes.Initialize(parent);
        }

        private void OnValidate()
        {
            if (_prevParent != parent)
            {
                _prevParent = parent;
                meshes.Initialize(parent);
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var mesh in meshes)
            {
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

        private static void RepaintRenderer<T>(T renderer) where T : Renderer
        {
            renderer.enabled = false;
            renderer.enabled = true;
        }

        [Button]
        private void Optimize()
        {
            foreach (var mesh in meshes)
            {
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
