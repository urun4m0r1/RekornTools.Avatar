#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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
        [SerializeField] public string ModifierLeft = "Left";
        [SerializeField] public string ModifierRight = "Right";
        [SerializeField] public string Splitter = ".";
        [SerializeField] public RigModifierPosition Position;

        public string Preview
        {
            get
            {
                if (Position == RigModifierPosition.Front)
                    return $"{ModifierLeft}{Splitter}Arm" +
                           $"{ModifierRight}{Splitter}Leg";
                else
                    return $"Arm{Splitter}{ModifierLeft}" +
                           $"Leg{Splitter}{ModifierRight}";
            }
        }
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
        [SerializeField] public bool IgnoreChildren;
        [SerializeField] public string Avatar;
        [SerializeField] public string Cloth;
    }


    [ExecuteInEditMode]
    public sealed class AutoDresser : MonoBehaviour
    {
        [SerializeField] private AvatarRig _avatar;
        [SerializeField] private AvatarRig _cloth;
        [SerializeField] private string _prefix;
        [SerializeField] private string _suffix;
        [SerializeField] private List<RigNamePair> _rigNameExceptions;

        public void Apply()
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
                var newChild = FindRecursive(_avatar.Rig.transform, child.name);
                if (newChild == null)
                {
                    Debug.LogError("Can't find " + child.name + " in " + _avatar.Rig.name);
                    continue;
                }

                var newName = _prefix + child.name + _suffix;
                child.name = newName;

                //set child parent to newChild's parent
                child.SetParent(newChild.parent);
                child.localPosition = newChild.localPosition;
                child.localRotation = newChild.localRotation;
                child.localScale = newChild.localScale;
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
