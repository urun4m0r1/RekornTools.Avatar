#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class ClothManager : MonoBehaviour
    {
        [Header("Cloth Settings")]
        [SerializeField] public List<SkinnedMeshRenderer> ClothMeshes = new List<SkinnedMeshRenderer>();
        [SerializeField] public List<Transform> ClothBones = new List<Transform>();
        [HideInInspector] public bool IsClothUnpacked = false;

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
