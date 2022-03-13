#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public sealed class BoneFinder : MonoBehaviour
    {
        [Header("Cloth Settings")]
        [SerializeField] public List<SkinnedMeshRenderer> ClothMeshes = new List<SkinnedMeshRenderer>();
        [SerializeField] public List<Transform> ClothBones = new List<Transform>();
        [HideInInspector] public bool IsClothUnpacked = false;
        [SerializeField] public string ClothName = "Cloth";

        [Button]
        public void RemoveCloth()
        {
            if (IsClothUnpacked)
            {
                foreach (var clothMesh in ClothMeshes)
                {
                    Undo.DestroyObjectImmediate(clothMesh.gameObject);
                }
            }
            else
            {
                Undo.DestroyObjectImmediate(ClothMeshes.First().transform.parent.gameObject);
            }

            Undo.RecordObject(this, "Remove Cloth");
            ClothMeshes.Clear();

            foreach (var clothBone in ClothBones)
            {
                if (clothBone != null)
                {
                    Undo.DestroyObjectImmediate(clothBone.gameObject);
                }
            }

            Undo.RecordObject(this, "Remove Cloth");
            ClothBones.Clear();

            Undo.DestroyObjectImmediate(gameObject);
        }

        [Button]
        public void FindActualBones()
        {
            Undo.RecordObject(this, "Remove Cloth");
            ClothBones.Clear();
            Undo.RecordObject(this, "Remove Cloth");
            foreach (SkinnedMeshRenderer clothMesh in ClothMeshes)
            {
                foreach (Transform bone in clothMesh.bones)
                {
                    if (IsRealBone(clothMesh, bone))
                    {
                        if (!ClothBones.Contains(bone))
                        {
                            ClothBones.Add(bone);
                        }
                    }
                }
            }
        }

        bool IsRealBone(SkinnedMeshRenderer skinnedMeshRenderer, Transform bone)
        {
            if (bone == null)
            {
                return false;
            }

            if (skinnedMeshRenderer.bones.Contains(bone))
            {
                return true;
            }

            return false;
        }

        [Button]
        public void SelectAllBones()
        {
            Selection.objects = ClothBones.Select(x => x.gameObject as Object).ToArray();
        }

        [Button]
        public void SelectAllMeshes()
        {
            Selection.objects = ClothMeshes.Select(x => x.gameObject as Object).ToArray();
        }
    }
}
#endif
