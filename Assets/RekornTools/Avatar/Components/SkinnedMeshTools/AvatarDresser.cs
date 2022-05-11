#if UNITY_EDITOR
using System;
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
        [Header("Avatar Settings")]
        [SerializeField] Animator _avatar;

        [Header("Cloth Settings")]
        [SerializeField] string _clothPrefix;
        [SerializeField] string       _clothSuffix;
        [SerializeField] Transform _cloth;

        [Header("Bone Exclusions")]
        [SerializeField] BonePairs _boneExclusions = new BonePairs();

        [Header("Advanced Settings")]
        [SerializeField] DresserMode _dresserMode = DresserMode.DirectTransform;
        [SerializeField] bool _backupCloth       = false;
        [SerializeField] bool _unpackClothMeshes = false;
        [SerializeField] bool _deleteLeftover    = true;

        [Button]
        public void Apply()
        {
            var armature = _cloth.Find("Armature");
            if (armature == null)
            {
                Debug.LogError("Can't find Armature in " + _cloth.name);
                return;
            }

            if (_backupCloth) BackupComponent(_cloth);
            else _cloth.gameObject.UnpackPrefab();

            var clothManagerGameObject = new GameObject($"{_cloth.name} Manager");
            Undo.RegisterCreatedObjectUndo(clothManagerGameObject, "Apply Cloth");
            Undo.SetTransformParent(clothManagerGameObject.transform, transform, "Cloth Manager");
            //var clothManager = Undo.AddComponent<ClothManager>(clothManagerGameObject);
            //clothManager.IsClothUnpacked = _unpackClothMeshes;

            var children = armature.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child == armature) continue;

                var childName     = child.name;
                var convertedName = childName;

                var newParent                    = _avatar.transform.FindRecursive(convertedName);
                if (newParent == null) newParent = child.parent;

                Undo.RecordObject(child.gameObject, "Apply Cloth");
                child.name = $"{_clothPrefix}{convertedName}{_clothSuffix}";

                Undo.SetTransformParent(child, newParent, "Parenting");

                //clothManager.ClothBones.Add(child);
            }

            var clothMeshes = _cloth.GetComponentsInChildren<SkinnedMeshRenderer>();
            //clothManager.ClothMeshes.AddRange(clothMeshes);

            Undo.SetTransformParent(_cloth, _avatar.transform, "Parenting");

            if (_unpackClothMeshes)
                foreach (var clothMesh in clothMeshes)
                    Undo.SetTransformParent(clothMesh.gameObject.transform, _avatar.transform, "Parenting");

            if (_deleteLeftover)
            {
                Undo.DestroyObjectImmediate(armature.gameObject);

                if (_unpackClothMeshes)
                    Undo.DestroyObjectImmediate(_cloth.gameObject);
                else
                    Undo.DestroyObjectImmediate(_cloth);
            }
        }

        void BackupComponent<T>(T component) where T : Component
        {
            var backup = Instantiate(component.gameObject);
            Undo.RegisterCreatedObjectUndo(backup, "Backup Cloth");
            Undo.RecordObject(this, "Assign Backup Cloth");
        }
    }
}
#endif
