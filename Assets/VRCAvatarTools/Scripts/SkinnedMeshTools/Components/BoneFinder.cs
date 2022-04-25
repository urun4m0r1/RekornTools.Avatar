#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshBonePairs))]
    public sealed class BoneFinder : MonoBehaviour
    {
        [field: SerializeField, Label(nameof(MeshParent))] [CanBeNull] public Transform MeshParent { get; set; }
        [field: SerializeField, Label(nameof(BoneParent))] [CanBeNull] public Transform BoneParent { get; set; }

        [field: SerializeField, Label(nameof(MeshKeyword))] [NotNull] public string MeshKeyword { get; set; } = "";
        [field: SerializeField, Label(nameof(BoneKeyword))] [NotNull] public string BoneKeyword { get; set; } = "";

        [NotNull] private SkinnedMeshRenderers Meshes => _meshBonePairs.Meshes;
        [NotNull] private Transforms           Bones  => _meshBonePairs.Bones;

        // ReSharper disable once NotNullMemberIsNotInitialized
        [NotNull] private MeshBonePairs _meshBonePairs;

        // ReSharper disable once AssignNullToNotNullAttribute
        private void Awake() => _meshBonePairs = GetComponent<MeshBonePairs>();

        public void FindMeshesFromTargetWithKeyword()
        {
            _meshBonePairs.UndoableAction(() => Meshes.Initialize(MeshParent, MeshKeyword));
            if (Meshes.Count == 0) this.ShowConfirmDialog("No objects found");
        }

        public void FindBonesFromTargetWithKeyword()
        {
            _meshBonePairs.UndoableAction(() =>
            {
                Bones.Initialize(BoneParent, BoneKeyword);
                RemoveMeshesFromBoneList();
            });

            if (Bones.Count == 0) this.ShowConfirmDialog("No objects found");
        }

        private void RemoveMeshesFromBoneList()
        {
            Bones.RemoveRange(Meshes.Select(x => x ? x.transform : null));
            Bones.Remove(BoneParent);
        }

        public void FindBonesFromMeshesWeightsIncludingChildren()
        {
            FindBonesFromMeshesWeights();

            var children = new List<Transform>();

            foreach (var child in
                     from b in Bones
                     where b && !Bones.Contains(b) && !children.Contains(b)
                     select b)
            {
                children.Add(child);
            }

            _meshBonePairs.UndoableAction(() => Bones.AddRange(children));
        }

        public void FindBonesFromMeshesWeights()
        {
            if (Meshes.Count == 0)
            {
                this.ShowConfirmDialog("There are no meshes to find bones from.");
                return;
            }

            var bones = new List<Transform>();

            foreach (var bone in
                     from m in Meshes
                     where m
                     from b in m.bones
                     where b && !bones.Contains(b)
                     select b)
            {
                bones.Add(bone);
            }

            _meshBonePairs.UndoableAction(() => Bones.Initialize(bones));

            if (Bones.Count == 0)
                this.ShowConfirmDialog("Failed to find any bones from meshes.\n" +
                                       "You might need to check meshes is valid or bone weights are not set to zero.");
        }
    }
}
#endif
