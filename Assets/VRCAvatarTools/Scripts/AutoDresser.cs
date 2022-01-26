#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace Rekorn.VRCAvatarTools
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

        public string PreviewText =>
            ModifierPosition == RigModifierPosition.Front
                ? $"{ModifierLeft}{Splitter}Arm / " +
                  $"{ModifierRight}{Splitter}Leg"
                : $"Arm{Splitter}{ModifierLeft} / " +
                  $"Leg{Splitter}{ModifierRight}";
    }

    [Serializable]
    public class RigNamingCombination
    {
        [SerializeField] public RigNamingConvention NamingConvention;
        public string LeftFront => $"{NamingConvention.ModifierLeft}{NamingConvention.Splitter}";
        public string RightFront => $"{NamingConvention.ModifierRight}{NamingConvention.Splitter}";
        public string LeftEnd => $"{NamingConvention.Splitter}{NamingConvention.ModifierLeft}";
        public string RightEnd => $"{NamingConvention.Splitter}{NamingConvention.ModifierRight}";

        public RigNamingCombination(RigNamingConvention namingConvention) => NamingConvention = namingConvention;
    }

    [Serializable]
    public struct AvatarRig
    {
        [SerializeField] public Animator Rig;
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
    public sealed class AutoDresser : MonoBehaviour
    {
        [Header("Rig Settings")]
        [SerializeField] private AvatarRig _avatar;
        [SerializeField] private AvatarRig _cloth;

        [Header("Dress Settings")]
        [SerializeField] private bool _unpackClothMeshes = true;
        [SerializeField] private bool _deleteLeftover = true;
        [SerializeField] private string _clothPrefix;
        [SerializeField] private string _clothSuffix;
        [SerializeField] private List<RigNamePair> _rigNameExceptions;

        [Header("Debug Info")]
        [ReadOnly] public bool DebugEnabled = true;
        [ShowNativeProperty] private string AvatarNamingPreview => _avatar.NamingConvention.PreviewText;
        [ShowNativeProperty] private string ClothNamingPreview => _cloth.NamingConvention.PreviewText;
        [ShowNativeProperty] private string NewClothNamingPreview => $"{_clothPrefix}Hips{_clothSuffix}";

        public void ChangeNamingConvention()
        {
            //find gameObject named "Armature" in _clothRoot and add all gameObject inside Armature prefix and suffix
            var armature = _cloth.Rig.transform.Find("Armature");
            if (armature == null)
            {
                Debug.LogError("Can't find Armature in " + _cloth.Rig.name);
                return;
            }

            // if _clothRoot is prefab, unpack it
            if (PrefabUtility.GetPrefabInstanceStatus(_cloth.Rig.gameObject) == PrefabInstanceStatus.Connected)
            {
                PrefabUtility.UnpackPrefabInstance(_cloth.Rig.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
            }

            var children = armature.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                var newName = child.name;
                var clothModifierPosition = _cloth.NamingConvention.ModifierPosition;
                var avatarModifierPosition = _avatar.NamingConvention.ModifierPosition;

                var clothCombination = new RigNamingCombination(_cloth.NamingConvention);
                var avatarCombination = new RigNamingCombination(_avatar.NamingConvention);

                if (clothModifierPosition == RigModifierPosition.Front)
                {
                    if (newName.Contains(clothCombination.LeftFront))
                    {
                        newName = newName.Replace(clothCombination.LeftFront, "");

                        if (avatarModifierPosition == RigModifierPosition.Front)
                        {
                            newName = $"{avatarCombination.LeftFront}{newName}";
                        }
                        else
                        {
                            newName = $"{newName}{avatarCombination.LeftEnd}";
                        }
                    }
                    else if (newName.Contains(clothCombination.RightFront))
                    {
                        newName = newName.Replace(clothCombination.RightFront, "");

                        if (avatarModifierPosition == RigModifierPosition.Front)
                        {
                            newName = $"{avatarCombination.RightFront}{newName}";
                        }
                        else
                        {
                            newName = $"{newName}{avatarCombination.RightEnd}";
                        }
                    }
                }
                else
                {
                    if (newName.Contains(clothCombination.LeftEnd))
                    {
                        newName = newName.Replace(clothCombination.LeftEnd, "");

                        if (avatarModifierPosition == RigModifierPosition.Front)
                        {
                            newName = $"{avatarCombination.LeftFront}{newName}";
                        }
                        else
                        {
                            newName = $"{newName}{avatarCombination.LeftEnd}";
                        }
                    }
                    else if (newName.Contains(clothCombination.RightEnd))
                    {
                        newName = newName.Replace(clothCombination.RightEnd, "");

                        if (avatarModifierPosition == RigModifierPosition.Front)
                        {
                            newName = $"{avatarCombination.RightFront}{newName}";
                        }
                        else
                        {
                            newName = $"{newName}{avatarCombination.RightEnd}";
                        }
                    }
                }

                foreach (var exception in _rigNameExceptions)
                {
                    if (newName.Contains(exception.ClothBoneName))
                    {
                        newName = newName.Replace(exception.ClothBoneName, exception.AvatarBoneName);
                    }
                }

                child.name = newName;
            }
        }

        public void ApplyCloth()
        {
            //find gameObject named "Armature" in _clothRoot and add all gameObject inside Armature prefix and suffix
            var armature = _cloth.Rig.transform.Find("Armature");
            if (armature == null)
            {
                Debug.LogError("Can't find Armature in " + _cloth.Rig.name);
                return;
            }

            // if _clothRoot is prefab, unpack it
            if (PrefabUtility.GetPrefabInstanceStatus(_cloth.Rig.gameObject) == PrefabInstanceStatus.Connected)
            {
                Debug.LogWarning("Unpack " + _cloth.Rig.name + " prefab first!");
                return;
            }

            var children = armature.GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                var disableParenting = false;

                foreach (var exception in _rigNameExceptions)
                {
                    if (child.name.Contains(exception.AvatarBoneName))
                    {
                        disableParenting = exception.DisableParenting;
                    }
                }

                var newParent = FindRecursive(_avatar.Rig.transform, child.name);
                if (newParent == null)
                {
                    newParent = child.parent;
                }

                Undo.RecordObject(child.gameObject, "Apply Cloth");
                child.name = $"{_clothPrefix}{child.name}{_clothSuffix}";

                if (disableParenting)
                {
                    Undo.SetTransformParent(child, newParent.parent, "Parenting");
                }
                else
                {
                    Undo.SetTransformParent(child, newParent, "Parenting");
                }
            }

            Undo.SetTransformParent(_cloth.Rig.transform, _avatar.Rig.transform, "Parenting");

            if (_unpackClothMeshes)
            {
                var clothMeshes = _cloth.Rig.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var clothMesh in clothMeshes)
                {
                    Undo.SetTransformParent(clothMesh.gameObject.transform, _avatar.Rig.transform, "Parenting");
                }
            }

            if (_deleteLeftover)
            {
                Undo.DestroyObjectImmediate(armature.gameObject);

                if (_unpackClothMeshes)
                {
                    Undo.DestroyObjectImmediate(_cloth.Rig.gameObject);
                }
                else
                {
                    Undo.DestroyObjectImmediate(_cloth.Rig);
                }
            }
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
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
#endif
