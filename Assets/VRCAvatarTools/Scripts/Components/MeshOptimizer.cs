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
        [SerializeField] Vector3                 boundingBoxCenter = Vector3.zero;
        [SerializeField] Vector3                 boundingBoxSize = Vector3.one;

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
                Bounds bounds = mesh.bounds;
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }

        Vector3 InverseElement(Vector3 value) => new Vector3(1.0f / value.x, 1.0f / value.y, 1.0f / value.z);

        [Button] void Optimize()
        {
            var meshBounds = new Bounds(boundingBoxCenter, boundingBoxSize * 2f);
            foreach (SkinnedMeshRenderer mesh in meshes)
            {
                Undo.RecordObject(mesh, nameof(MeshOptimizer));
                {
                    mesh.probeAnchor               = anchorOverride;
                    mesh.localBounds               = meshBounds;
                    mesh.updateWhenOffscreen       = false;
                    mesh.skinnedMotionVectors      = false;
                    mesh.allowOcclusionWhenDynamic = true;
                }
            }
        }
    }
}
