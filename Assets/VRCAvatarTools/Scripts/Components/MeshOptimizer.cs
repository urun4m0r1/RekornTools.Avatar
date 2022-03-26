using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public class MeshOptimizer : MonoBehaviour
    {
        [SerializeField] Transform               parent;
        [SerializeField] SkinnedMeshRendererList meshes = new SkinnedMeshRendererList();
        [SerializeField] Transform               anchorOverride;
        [SerializeField] Bounds                  boundingBox;

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
            foreach (SkinnedMeshRenderer mesh in meshes)
            {
                Bounds prevBounds = mesh.localBounds;
                Bounds bounds     = mesh.bounds;

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

        static void RepaintRenderer<T>(T renderer) where T : Renderer
        {
            renderer.enabled = false;
            renderer.enabled = true;
        }

        [Button] void Optimize()
        {
            foreach (SkinnedMeshRenderer mesh in meshes)
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
