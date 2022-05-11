#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    public enum DresserMode
    {
        DirectTransform,
        ParentConstraint,
        WeightTransfer,
    }

    [ExecuteInEditMode]
    public sealed class AvatarDresser : MonoBehaviour
    {
        [SerializeField] DresserMode _dresserMode = DresserMode.DirectTransform;

        [Header("Avatar Settings")]
        [SerializeField] Animator _avatar;

        [Header("Cloth Settings")]
        [SerializeField] string _clothPrefix;
        [SerializeField] string    _clothSuffix;
        [SerializeField] Transform _clothRoot;
        [SerializeField] Transform _clothArmature;

        [Header("Bone Exclusions")]
        [SerializeField] [NotNull] BonePairs _boneExclusions = new BonePairs();

        void OnValidate()
        {
            if (_clothRoot != null && _clothArmature == null) _clothArmature = _clothRoot.Find("Armature");
        }

        [Button]
        public void Apply()
        {
            switch (_dresserMode)
            {
                case DresserMode.DirectTransform:
                    ApplyDirectTransform();
                    break;
                case DresserMode.ParentConstraint:
                    ApplyParentConstraint();
                    break;
                case DresserMode.WeightTransfer:
                    ApplyWeightTransfer();
                    break;
            }
        }

        void ApplyDirectTransform()
        {
            if (_avatar == null || _clothRoot == null)
            {
                this.ShowConfirmDialog("Avatar or cloth is not set.");
                return;
            }

            if (_clothArmature == null)
            {
                this.ShowConfirmDialog("Cloth armature is not set.");
                return;
            }

            _clothRoot.gameObject.UnpackPrefab();

            var bones  = _clothArmature.GetComponentsInChildren<Transform>();
            var meshes = _clothRoot.GetComponentsInChildren<SkinnedMeshRenderer>();

            if (bones == null || meshes == null)
            {
                this.ShowConfirmDialog("No bones or meshes found.");
                Undo.PerformUndo();
                return;
            }

            var result = CreateMeshBonePairs(_clothRoot.name, transform);
            result.Bones.AddRange(bones);
            result.Meshes.AddRange(meshes);

            TransferBones(bones, _avatar.transform);
            TransferExcludedBones(_boneExclusions);
            RenameBones(bones, _clothPrefix, _clothSuffix);
            TransferMeshes(_clothRoot, _avatar.transform);
        }

        void ApplyParentConstraint()
        {
            this.ShowConfirmDialog("ParentConstraint is not implemented yet.");
        }


        void ApplyWeightTransfer()
        {
            this.ShowConfirmDialog("WeightTransfer is not implemented yet.");
        }

        [NotNull]
        static MeshBonePairs CreateMeshBonePairs([NotNull] string title, [CanBeNull] Transform parent)
        {
            var undo = "CreateMeshBonePairs";
            {
                var go = new GameObject($"[{title}]");
                Undo.RegisterCreatedObjectUndo(go, undo);
                Undo.SetTransformParent(go.transform, parent, undo);
                return Undo.AddComponent<MeshBonePairs>(go) ?? throw new NullReferenceException(undo);
            }
        }

        static void TransferBones([NotNull] IEnumerable<Transform> bones, [NotNull] Transform targetParent)
        {
            var undo = "TransferBones";
            foreach (var bone in bones)
            {
                if (bone == null) continue;

                var avatarBone = targetParent.FindRecursive(bone.name);
                if (avatarBone != null) Undo.SetTransformParent(bone, avatarBone, undo);
            }
        }

        static void TransferExcludedBones([NotNull] BonePairs boneExclusions)
        {
            var undo = "TransferExcludedBones";
            foreach (var exclusion in boneExclusions)
            {
                if (exclusion == null || exclusion.Source == null) continue;
                Undo.SetTransformParent(exclusion.Source, exclusion.Target, undo);
            }
        }

        static void RenameBones([NotNull] IEnumerable<Transform> bones, [CanBeNull] string prefix, [CanBeNull] string suffix)
        {
            var undo = "RenameBones";
            foreach (var bone in bones)
            {
                if (bone == null) continue;
                bone.UndoableAction(undo, () => bone.name = $"{prefix}{bone.name}{suffix}");
            }
        }

        static void TransferMeshes([NotNull] Transform source, [CanBeNull] Transform parent)
        {
            var undo = "TransferMeshes";
            Undo.SetTransformParent(source, parent, undo);
        }
    }
}
#endif
