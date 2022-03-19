using NaughtyAttributes;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public class MeshOptimizer : MonoBehaviour
    {
        [SerializeField] Transform               parent;
        [SerializeField] SkinnedMeshRendererList meshes = new SkinnedMeshRendererList();
        [SerializeField] Transform               anchorOverride;
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

        [Button] void Optimize()
        {
            var meshBounds = new Bounds(Vector3.zero, boundingBoxSize * 2f);
            foreach (SkinnedMeshRenderer mesh in meshes)
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
