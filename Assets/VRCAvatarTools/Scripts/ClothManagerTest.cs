#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public sealed class ClothManagerTest : MonoBehaviour
    {
        [SerializeField] ClothManager clothManagerA;
        [SerializeField] ClothManager clothManagerB;
        [SerializeField] List<Transform> diffs;

        [Button]
        public void SelectDiff()
        {
            diffs.Clear();
            clothManagerA.ClothBones.ForEach(bone =>
            {
                if (!clothManagerB.ClothBones.Contains(bone))
                {
                    if (!diffs.Contains(bone))
                        diffs.Add(bone);
                }
            });
        }
    }
}
#endif
