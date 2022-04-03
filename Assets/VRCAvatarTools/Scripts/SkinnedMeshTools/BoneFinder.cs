#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshBonePairs))]
    public sealed class BoneFinder : MonoBehaviour
    {
        [field: SerializeField, Label(nameof(MeshParent))]  public Transform MeshParent  { get; set; }
        [field: SerializeField, Label(nameof(BoneParent))]  public Transform BoneParent  { get; set; }
        [field: SerializeField, Label(nameof(MeshKeyword))] public string    MeshKeyword { get; set; }
        [field: SerializeField, Label(nameof(BoneKeyword))] public string    BoneKeyword { get; set; }

        const     string ClassName = nameof(BoneFinder);
        [NotNull] string GameObjectName => gameObject.name;
        [NotNull] string Header         => $"[{ClassName}({GameObjectName})]";

        SkinnedMeshRendererList _meshList;
        TransformList           _boneList;

        void ShowDialog(string message)
        {
            Debug.LogWarning($"{Header} {message}");
            EditorUtility.DisplayDialog(
                Header,
                message,
                "Confirm");
        }

        MeshBonePairs _meshBonePairs;

        void Awake()
        {
            _meshBonePairs = GetComponent<MeshBonePairs>();
            _meshList      = _meshBonePairs.Meshes; //TODO:  레퍼런스를 복사야야함
            _boneList      = _meshBonePairs.Bones;
        }

        public void FindMeshesFromTargetWithKeyword()
        {
            Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
            {
                _meshBonePairs.Meshes.Initialize(MeshParent, MeshKeyword);
            }
            if (_meshBonePairs.Meshes.Count == 0) ShowDialog("No objects found");
        }

        public void FindBonesFromTargetWithKeyword()
        {
            Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
            {
                _meshBonePairs.Bones.Initialize(BoneParent, BoneKeyword);
                _meshBonePairs.Bones.RemoveRange(_meshBonePairs.Meshes.Select(x => x.transform).ToList());
                _meshBonePairs.Bones.Remove(BoneParent);
            }
            if (_meshBonePairs.Bones.Count == 0) ShowDialog("No objects found");
        }

        public void FindBonesFromMeshesWeightsIncludingChildren()
        {
            FindBonesFromMeshesWeights();

            var children = new List<Transform>();

            foreach (Transform child in
                     from b in _meshBonePairs.Bones
                     from Transform c in b
                     where c && !_meshBonePairs.Bones.Contains(c) && !children.Contains(c)
                     select c)
            {
                children.Add(child);
            }

            Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
            {
                _meshBonePairs.Bones.AddRange(children);
            }
        }

        public void FindBonesFromMeshesWeights()
        {
            if (_meshBonePairs.Meshes.Count == 0)
            {
                ShowDialog("There are no meshes to find bones from.");
                return;
            }

            Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
            {
                _meshBonePairs.Bones.Clear();
            }

            foreach (Transform bone in
                     from m in _meshBonePairs.Meshes
                     where m
                     from b in m.bones
                     where b && !_meshBonePairs.Bones.Contains(b)
                     select b)
            {
                Undo.RegisterCompleteObjectUndo(_meshBonePairs, ClassName);
                {
                    _meshBonePairs.Bones.Add(bone);
                }
            }

            if (_meshBonePairs.Bones.Count == 0)
            {
                ShowDialog("Failed to find any bones from meshes.\n" +
                           "You might need to check meshes is valid or bone weights are not set to zero.");
            }
        }
    }
}
#endif
