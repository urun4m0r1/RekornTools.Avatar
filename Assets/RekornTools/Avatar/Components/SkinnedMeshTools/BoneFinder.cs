﻿#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshBonePairs))]
    public sealed class BoneFinder : MonoBehaviour
    {
        [field: SerializeField] [CanBeNull] public Transform MeshParent { get; set; }
        [field: SerializeField] [CanBeNull] public Transform BoneParent { get; set; }

        [field: SerializeField] [CanBeNull] public string MeshKeyword { get; set; }
        [field: SerializeField] [CanBeNull] public string BoneKeyword { get; set; }

        [NotNull] public SkinnedMeshRenderers Meshes => _meshBonePairs.Meshes;
        [NotNull] public Transforms           Bones  => _meshBonePairs.Bones;

        // ReSharper disable once NotNullMemberIsNotInitialized
        [NotNull] MeshBonePairs _meshBonePairs;

        // ReSharper disable once AssignNullToNotNullAttribute
        void Awake() => _meshBonePairs = GetComponent<MeshBonePairs>();

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
                Bones.RemoveRange(Meshes.Select(x => x == null ? null : x.transform));
                Bones.Remove(BoneParent);
            });

            if (Bones.Count == 0) this.ShowConfirmDialog("No objects found");
        }

        public void FindBonesFromWeights()
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
                     where b != null && !bones.Contains(b)
                     select b)
            {
                bones.Add(bone);
            }

            _meshBonePairs.UndoableAction(() => Bones.Initialize(bones));

            if (Bones.Count == 0)
                this.ShowConfirmDialog("Failed to find any bones from meshes.\n" +
                                       "You might need to check meshes is valid or bone weights are not set to zero.");
        }

        public void FindBonesFromWeightsRecursive()
        {
            FindBonesFromWeights();

            var children = GetChildrenRecursive(Bones);
            _meshBonePairs.UndoableAction(() => Bones.AddRange(children));
        }

        [NotNull]
        static List<Transform> GetChildrenRecursive([NotNull] Transforms target)
        {
            var children = new List<Transform>();

            foreach (var child in
                     from t in target
                     where t != null
                     select t.GetComponentsInChildren<Transform>(true))
            {
                if (child == null) continue;
                children.AddRange(child.Where(c => !AlreadyInList(c)));
            }

            return children;

            bool AlreadyInList(Transform b) => target.Contains(b) || children.Contains(b);
        }
    }
}
#endif