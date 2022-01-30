#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public enum RigModifierPosition
    {
        Front,
        End,
    }

    [Serializable]
    public class RigNamingConvention
    {
        [SerializeField] public RigModifierPosition ModifierPosition;
        [SerializeField] public string Splitter = ".";
        [SerializeField] public string ModifierLeft = "Left";
        [SerializeField] public string ModifierRight = "Right";

        public string LeftFront => $"{ModifierLeft}{Splitter}";
        public string RightFront => $"{ModifierRight}{Splitter}";
        public string LeftEnd => $"{Splitter}{ModifierLeft}";
        public string RightEnd => $"{Splitter}{ModifierRight}";

        public string PreviewText =>
            ModifierPosition == RigModifierPosition.Front
                ? $"{LeftFront}Arm / {RightFront}Leg"
                : $"Arm{LeftEnd} / Leg{RightEnd}";
    }

    [Serializable]
    public struct AvatarRig
    {
        [SerializeField] public Animator Rig;

        private bool IsValid => Rig != null;
        [ShowIf("IsValid"), AllowNesting]
        [SerializeField] public RigNamingConvention NamingConvention;
    }

    [Serializable]
    public struct RigNamePair
    {
        [SerializeField] public bool DisableParenting;
        [SerializeField] public string AvatarBoneName;
        [SerializeField] public string ClothBoneName;
    }

    [ExecuteInEditMode]
    public sealed partial class AutoDresser : MonoBehaviour
    {
        [Header("Rig Settings")]
        [SerializeField] private AvatarRig _avatar;
        [SerializeField] private AvatarRig _cloth;

        [Header("Dress Settings")]
        [SerializeField] private bool _backupCloth = false;
        [SerializeField] private bool _unpackClothMeshes = false;
        [SerializeField] private bool _deleteLeftover = true;
        [SerializeField] private string _clothPrefix;
        [SerializeField] private string _clothSuffix;
        [SerializeField] private List<RigNamePair> _rigNameExceptions;

        [Header("Debug Info")]
        [ReadOnly] public bool DebugEnabled = true;
        [ShowNativeProperty] private string AvatarNamingPreview => _avatar.NamingConvention.PreviewText;
        [ShowNativeProperty] private string ClothNamingPreview => _cloth.NamingConvention.PreviewText;
        [ShowNativeProperty] private string NewClothNamingPreview => $"{_clothPrefix}Hips{_clothSuffix}";

        private string ChangeNamingConvention(string oldName)
        {
            var newName = oldName;

            if (_cloth.NamingConvention.ModifierPosition == RigModifierPosition.Front)
            {
                var clothLeftFront = _cloth.NamingConvention.LeftFront;
                var clothRightFront = _cloth.NamingConvention.RightFront;
                if (!ReplaceAndRenameIfExist(clothLeftFront, RenameLeftNewName))
                    ReplaceAndRenameIfExist(clothRightFront, RenameRightNewName);
            }
            else
            {
                var clothLeftEnd = _cloth.NamingConvention.LeftEnd;
                var clothRightEnd = _cloth.NamingConvention.RightEnd;
                if (!ReplaceAndRenameIfExist(clothLeftEnd, RenameLeftNewName))
                    ReplaceAndRenameIfExist(clothRightEnd, RenameRightNewName);
            }

            foreach (var exception in _rigNameExceptions)
            {
                if (newName.Contains(exception.ClothBoneName))
                {
                    newName = newName.Replace(exception.ClothBoneName, exception.AvatarBoneName);
                }
            }

            return newName;

            bool ReplaceAndRenameIfExist(string str, Action renameAction)
            {
                if (newName.Contains(str))
                {
                    newName = newName.Replace(str, "");
                    renameAction();
                    return true;
                }
                return false;
            }

            void RenameLeftNewName()
            {
                var avatarLeftFront = _avatar.NamingConvention.LeftFront;
                var avatarLeftEnd = _avatar.NamingConvention.LeftEnd;
                SetNewName(avatarLeftFront, avatarLeftEnd);
            }

            void RenameRightNewName()
            {
                var avatarRightFront = _avatar.NamingConvention.RightFront;
                var avatarRightEnd = _avatar.NamingConvention.RightEnd;
                SetNewName(avatarRightFront, avatarRightEnd);
            }

            void SetNewName(string avatarFront, string avatarEnd)
            {
                if (_avatar.NamingConvention.ModifierPosition == RigModifierPosition.Front)
                    newName = $"{avatarFront}{newName}";
                else
                    newName = $"{newName}{avatarEnd}";
            }
        }

        [Button]
        public void ApplyCloth()
        {
            var armature = _cloth.Rig.transform.Find("Armature");
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

                var disableParenting = false;

                child.name = ChangeNamingConvention(child.name);

                foreach (var exception in _rigNameExceptions)
                    if (child.name.Contains(exception.AvatarBoneName))
                        disableParenting = exception.DisableParenting;

                var newParent = FindRecursive(_avatar.Rig.transform, child.name);
                if (newParent == null) newParent = child.parent;

                Undo.RecordObject(child.gameObject, "Apply Cloth");
                child.name = $"{_clothPrefix}{child.name}{_clothSuffix}";

                Undo.SetTransformParent(child, disableParenting ? newParent.parent : newParent, "Parenting");

                clothManager.ClothBones.Add(child);
            }

            var clothMeshes = _cloth.Rig.GetComponentsInChildren<SkinnedMeshRenderer>();
            clothManager.ClothMeshes.AddRange(clothMeshes);

            Undo.SetTransformParent(_cloth.Rig.transform, _avatar.Rig.transform, "Parenting");

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

        private void BackupComponent<T>(ref T component) where T : Component
        {
            var backup = Instantiate(component.gameObject);
            Undo.RegisterCreatedObjectUndo(backup, "Backup Cloth");
            Undo.RecordObject(this, "Assign Backup Cloth");
            component = backup.GetComponent<T>();
        }

        private static void UnpackPrefab(GameObject prefab)
        {
            if (PrefabUtility.GetPrefabInstanceStatus(prefab) == PrefabInstanceStatus.Connected)
                PrefabUtility.UnpackPrefabInstance(prefab, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }

        private static Transform FindRecursive(Transform root, string name)
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
