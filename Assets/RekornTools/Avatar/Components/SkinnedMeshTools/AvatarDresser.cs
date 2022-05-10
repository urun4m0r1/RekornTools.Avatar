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
        [SerializeField] AnimatorRig _avatar;

        [Header("Cloth Settings")]
        [SerializeField] string _clothPrefix;
        [SerializeField] string       _clothSuffix;
        [SerializeField] TransformRig _cloth;

        [Header("Rename Token")]
        [SerializeField] [ItemNotSpan] NamePairs _nameExclusions = new NamePairs();

        [Header("Bone Exclusions")]
        [SerializeField] [ItemNotSpan] BonePairs _boneExclusions = new BonePairs();

        [Header("Advanced Settings")]
        [SerializeField] DresserMode _dresserMode = DresserMode.DirectTransform;
        [SerializeField] bool _backupCloth       = false;
        [SerializeField] bool _unpackClothMeshes = false;
        [SerializeField] bool _deleteLeftover    = true;

        [Button]
        public void Apply()
        {
            var armature = _cloth.Rig.Find("Armature");
            if (armature == null)
            {
                Debug.LogError("Can't find Armature in " + _cloth.Rig.name);
                return;
            }

            if (_backupCloth) BackupComponent(_cloth.Rig);
            else _cloth.Rig.gameObject.UnpackPrefab();

            var clothManagerGameObject = new GameObject($"{_cloth.Rig.name} Manager");
            Undo.RegisterCreatedObjectUndo(clothManagerGameObject, "Apply Cloth");
            Undo.SetTransformParent(clothManagerGameObject.transform, transform, "Cloth Manager");
            //var clothManager = Undo.AddComponent<ClothManager>(clothManagerGameObject);
            //clothManager.IsClothUnpacked = _unpackClothMeshes;

            var children = armature.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child == armature) continue;

                var childName = child.name;
                var convertedName =
                    RigNamingConvention.Convert(childName, _cloth.Naming, _avatar.Naming);

                var newParent                    = FindRecursive(_avatar.Rig.transform, convertedName);
                if (newParent == null) newParent = child.parent;

                Undo.RecordObject(child.gameObject, "Apply Cloth");
                child.name = $"{_clothPrefix}{convertedName}{_clothSuffix}";

                Undo.SetTransformParent(child, newParent, "Parenting");

                //clothManager.ClothBones.Add(child);
            }

            var clothMeshes = _cloth.Rig.GetComponentsInChildren<SkinnedMeshRenderer>();
            //clothManager.ClothMeshes.AddRange(clothMeshes);

            Undo.SetTransformParent(_cloth.Rig, _avatar.Rig.transform, "Parenting");

            if (_unpackClothMeshes)
                foreach (var clothMesh in clothMeshes)
                    Undo.SetTransformParent(clothMesh.gameObject.transform, _avatar.Rig.transform, "Parenting");

            if (_deleteLeftover)
            {
                Undo.DestroyObjectImmediate(armature.gameObject);

                if (_unpackClothMeshes)
                    Undo.DestroyObjectImmediate(_cloth.Rig.gameObject);
                else
                    Undo.DestroyObjectImmediate(_cloth.Rig);
            }
        }

        void BackupComponent<T>(T component) where T : Component
        {
            var backup = Instantiate(component.gameObject);
            Undo.RegisterCreatedObjectUndo(backup, "Backup Cloth");
            Undo.RecordObject(this, "Assign Backup Cloth");
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
