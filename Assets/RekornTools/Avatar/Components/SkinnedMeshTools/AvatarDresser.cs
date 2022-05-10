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

        [Header("Exclusion Settings")]
        [SerializeField] [ItemNotSpan] RigPairs _rigExclusions = new RigPairs();

        [Header("Advanced Settings")]
        [SerializeField] bool _backupCloth = false;
        [SerializeField] bool        _unpackClothMeshes = false;
        [SerializeField] bool        _deleteLeftover    = true;
        [SerializeField] DresserMode _dresserMode       = DresserMode.DirectTransform;

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
            else UnpackPrefab(_cloth.Rig.gameObject);

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

                var disableParenting = false;
                convertedName = ConvertNameExceptions(convertedName, ref disableParenting);

                var newParent = FindRecursive(_avatar.Rig.transform, convertedName);
                if (newParent == null) newParent     = child.parent;
                else if (disableParenting) newParent = child.parent.parent;

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

        string ConvertNameExceptions(string str, ref bool disableParenting)
        {
            // foreach (var exception in _rigNameExceptions.Where(exception => str.Contains(exception.ClothBoneName)))
            // {
            //     Rename(ref str, exception.ClothBoneName, exception.AvatarBoneName);
            //     disableParenting = exception.DisableParenting;
            // }

            return str;
        }

        void BackupComponent<T>(T component) where T : Component
        {
            var backup = Instantiate(component.gameObject);
            Undo.RegisterCreatedObjectUndo(backup, "Backup Cloth");
            Undo.RecordObject(this, "Assign Backup Cloth");
        }

        static void UnpackPrefab(GameObject prefab)
        {
            if (PrefabUtility.GetPrefabInstanceStatus(prefab) == PrefabInstanceStatus.Connected)
                PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
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
