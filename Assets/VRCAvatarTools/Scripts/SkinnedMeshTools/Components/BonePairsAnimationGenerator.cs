#if UNITY_EDITOR
using JetBrains.Annotations;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshBonePairs))]
    public sealed class BonePairsAnimationGenerator : MonoBehaviour
    {
        [NotNull] private SkinnedMeshRenderers Meshes => _meshBonePairs.Meshes;
        [NotNull] private Transforms           Bones  => _meshBonePairs.Bones;

        // ReSharper disable once NotNullMemberIsNotInitialized
        [NotNull] private MeshBonePairs _meshBonePairs;

        // ReSharper disable once AssignNullToNotNullAttribute
        private void Awake() => _meshBonePairs = GetComponent<MeshBonePairs>();

        public void CreatePairsToggleAnimation()   { }
        public void CreatePairsEnableAnimation()   { }
        public void CreatePairsDisableAnimation()  { }
        public void CreateMeshesToggleAnimation()  { }
        public void CreateMeshesEnableAnimation()  { }
        public void CreateMeshesDisableAnimation() { }
        public void CreateBonesToggleAnimation()   { }
        public void CreateBonesEnableAnimation()   { }
        public void CreateBonesDisableAnimation()  { }
    }
}
#endif
