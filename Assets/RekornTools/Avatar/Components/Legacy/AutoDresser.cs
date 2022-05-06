#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    public enum RigModifierPosition
    {
        Front,
        End,
    }

    [Serializable]
    public struct RigNamingConvention
    {
        [SerializeField] public RigModifierPosition ModifierPosition;
        [SerializeField] public string              Splitter;
        [SerializeField] public string              ModifierLeft;
        [SerializeField] public string              ModifierRight;

        public string LeftFront  => $"{ModifierLeft}{Splitter}";
        public string RightFront => $"{ModifierRight}{Splitter}";
        public string LeftEnd    => $"{Splitter}{ModifierLeft}";
        public string RightEnd   => $"{Splitter}{ModifierRight}";

        public string PreviewText =>
            ModifierPosition == RigModifierPosition.Front
                ? $"{LeftFront}Arm / {RightFront}Leg"
                : $"Arm{LeftEnd} / Leg{RightEnd}";
    }

    [Serializable]
    public struct AvatarRig
    {
        [SerializeField] public Transform           Rig;
        [SerializeField] public RigNamingConvention NamingConvention;
    }

    [Serializable]
    public struct RigNamePair
    {
        [SerializeField] public bool   DisableParenting;
        [SerializeField] public string AvatarBoneName;
        [SerializeField] public string ClothBoneName;
    }

    [ExecuteInEditMode]
    public sealed partial class AutoDresser : MonoBehaviour
    {
        [Header("Rig Settings")]
        [SerializeField]
        AvatarRig _avatar;

        [SerializeField] AvatarRig _cloth;

        [Header("Dress Settings")]
        [SerializeField]
        bool _backupCloth = false;

        [SerializeField] bool              _unpackClothMeshes = false;
        [SerializeField] bool              _deleteLeftover    = true;
        [SerializeField] string            _clothPrefix;
        [SerializeField] string            _clothSuffix;
        [SerializeField] List<RigNamePair> _rigNameExceptions;

        static string ConvertNamingConvention(string name, RigNamingConvention src, RigNamingConvention dst)
        {
            var newName = name;

            if (src.ModifierPosition == RigModifierPosition.Front)
            {
                if (!ReplaceAndRenameIfExist(src.LeftFront, RenameLeft))
                    ReplaceAndRenameIfExist(src.RightFront, RenameRight);
            }
            else
            {
                if (!ReplaceAndRenameIfExist(src.LeftEnd, RenameLeft))
                    ReplaceAndRenameIfExist(src.RightEnd, RenameRight);
            }

            return newName;

            bool ReplaceAndRenameIfExist(string str, Action renameAction)
            {
                if (newName.Contains(str))
                {
                    Rename(ref newName, str, "");
                    renameAction();
                    return true;
                }

                return false;
            }

            void RenameLeft()
            {
                var avatarLeftFront = dst.LeftFront;
                var avatarLeftEnd   = dst.LeftEnd;
                SetNewName(avatarLeftFront, avatarLeftEnd);
            }

            void RenameRight()
            {
                var avatarRightFront = dst.RightFront;
                var avatarRightEnd   = dst.RightEnd;
                SetNewName(avatarRightFront, avatarRightEnd);
            }

            void SetNewName(string avatarFront, string avatarEnd)
            {
                if (dst.ModifierPosition == RigModifierPosition.Front)
                    newName = $"{avatarFront}{newName}";
                else
                    newName = $"{newName}{avatarEnd}";
            }
        }

        [Button]
        public void ApplyCloth()
        {
            var armature = _cloth.Rig.Find("Armature");
            if (armature == null)
            {
                Debug.LogError("Can't find Armature in " + _cloth.Rig.name);
                return;
            }

            if (_backupCloth) BackupComponent(ref _cloth.Rig);
            else UnpackPrefab(_cloth.Rig.gameObject);

            var clothManagerGameObject = new GameObject($"{_cloth.Rig.name} Manager");
            Undo.RegisterCreatedObjectUndo(clothManagerGameObject, "Apply Cloth");
            Undo.SetTransformParent(clothManagerGameObject.transform, transform, "Cloth Manager");
            var clothManager = Undo.AddComponent<ClothManager>(clothManagerGameObject);
            clothManager.IsClothUnpacked = _unpackClothMeshes;

            var children = armature.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child == armature) continue;

                var childName = child.name;
                var convertedName =
                    ConvertNamingConvention(childName, _cloth.NamingConvention, _avatar.NamingConvention);

                var disableParenting = false;
                convertedName = ConvertNameExceptions(convertedName, ref disableParenting);

                var newParent = FindRecursive(_avatar.Rig, convertedName);
                if (newParent == null) newParent     = child.parent;
                else if (disableParenting) newParent = child.parent.parent;

                Undo.RecordObject(child.gameObject, "Apply Cloth");
                child.name = $"{_clothPrefix}{convertedName}{_clothSuffix}";

                Undo.SetTransformParent(child, newParent, "Parenting");

                clothManager.ClothBones.Add(child);
            }

            var clothMeshes = _cloth.Rig.GetComponentsInChildren<SkinnedMeshRenderer>();
            clothManager.ClothMeshes.AddRange(clothMeshes);

            Undo.SetTransformParent(_cloth.Rig, _avatar.Rig, "Parenting");

            if (_unpackClothMeshes)
                foreach (var clothMesh in clothMeshes)
                    Undo.SetTransformParent(clothMesh.gameObject.transform, _avatar.Rig, "Parenting");

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
            foreach (var exception in _rigNameExceptions.Where(exception => str.Contains(exception.ClothBoneName)))
            {
                Rename(ref str, exception.ClothBoneName, exception.AvatarBoneName);
                disableParenting = exception.DisableParenting;
            }

            return str;
        }

        void BackupComponent<T>(ref T component) where T : Component
        {
            var backup = Instantiate(component.gameObject);
            Undo.RegisterCreatedObjectUndo(backup, "Backup Cloth");
            Undo.RecordObject(this, "Assign Backup Cloth");
            component = backup.GetComponent<T>();
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

        static void Rename(ref string str, string a, string b)
        {
            str = str.Replace(a, b);
        }
    }
}
#endif
