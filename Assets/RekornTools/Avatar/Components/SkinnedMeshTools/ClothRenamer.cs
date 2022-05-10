#if UNITY_EDITOR
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class ClothRenamer : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] Transform _parent;
        [SerializeField] RigNamingConvention _sourceNaming = RigNamingConvention.Default;

        [Header("Target")]
        [SerializeField] RigNamingConvention _targetNaming = RigNamingConvention.Default;

        [Header("Rename Token")]
        [SerializeField] NamePairs _renameToken = new NamePairs();

        [Header("Rename Exclusions")]
        [SerializeField] Transforms _exclusions = new Transforms();

        [Button]
        public void Apply()
        {
            var armature = _parent.Find("Armature");
            if (armature == null)
            {
                Debug.LogError("Can't find Armature in " + _parent.name);
                return;
            }

            var children = armature.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child == armature) continue;

                var childName     = child.name;
                var convertedName = childName;

                var newParent                    = FindRecursive(_parent, convertedName);
                if (newParent == null) newParent = child.parent;
            }
        }

        static Transform FindRecursive(Transform root, string name)
        {
            if (root.name == name)
            {
                return root;
            }

            foreach (Transform child in root)
            {
                var result = FindRecursive(child, name);
                if (result != null) return result;
            }

            return null;
        }
    }
}
#endif
