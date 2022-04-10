#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshBonePairs))]
    public sealed class BoneFinder : MonoBehaviour
    {
        private const string ClassName = nameof(BoneFinder);

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
            _meshBonePairs.UndoableAction(ClassName, () => Meshes.Initialize(MeshParent, MeshKeyword));
            if (Meshes.Count == 0) this.ShowConfirmDialog("No objects found");
        }

        public void FindBonesFromTargetWithKeyword()
        {
            Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
            {
                Bones.Initialize(BoneParent, BoneKeyword);
                Bones.RemoveRange(Meshes.Select(x => x ? x.transform : null));
                Bones.Remove(BoneParent);
            }
            if (Bones.Count == 0) this.ShowConfirmDialog("No objects found");
        }

        public void FindBonesFromMeshesWeightsIncludingChildren()
        {
            FindBonesFromMeshesWeights();

            var children = new List<Transform>();

            foreach (var child in
                     from b in Bones
                     from Transform c in b
                     where c && !Bones.Contains(c) && !children.Contains(c)
                     select c)
            {
                children.Add(child);
            }

            Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
            {
                Bones.AddRange(children);
            }
        }

        public void FindBonesFromMeshesWeights()
        {
            if (Meshes.Count == 0)
            {
                this.ShowConfirmDialog("There are no meshes to find bones from.");
                return;
            }

            Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
            {
                Bones.Clear();
            }

            foreach (var bone in
                     from m in Meshes
                     where m
                     from b in m.bones
                     where b && !Bones.Contains(b)
                     select b)
            {
                Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
                {
                    Bones.Add(bone);
                }
            }

            if (Bones.Count == 0)
            {
                this.ShowConfirmDialog("Failed to find any bones from meshes.\n" +
                                       "You might need to check meshes is valid or bone weights are not set to zero.");
            }
        }
    }
}
#endif
