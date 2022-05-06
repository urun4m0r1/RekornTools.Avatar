﻿#if UNITY_EDITOR
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class MeshBonePairs : MonoBehaviour
    {
        [field: SerializeField] [NotNull] public SkinnedMeshRenderers Meshes { get; private set; } = new SkinnedMeshRenderers();

        [field: SerializeField] [NotNull] public Transforms Bones { get; private set; } = new Transforms();

        const string ClassName = nameof(BoneFinder);

        public void SelectAll()
        {
            Object[] selection = { };

            if (Meshes.TryGetSelections(out var meshSelections))
                selection = selection.Concat(meshSelections).ToArray();
            if (Bones.TryGetSelections(out var boneSelections))
                selection = selection.Concat(boneSelections).ToArray();

            if (selection.Length > 0) Selection.objects = selection;
        }

        public void SelectMeshes() => Meshes.SelectComponents();
        public void SelectBones() => Bones.SelectComponents();

        public void ClearAll()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            {
                Meshes.Clear();
                Bones.Clear();
            }
        }

        public void ClearMeshes()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            {
                Meshes.Clear();
            }
        }

        public void ClearBones()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            {
                Bones.Clear();
            }
        }

        public void DestroyAll()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            {
                Meshes.DestroyItems();
                Bones.DestroyItems();
            }
        }

        public void DestroyMeshes()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            {
                Meshes.DestroyItems();
            }
        }

        public void DestroyBones()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            {
                Bones.DestroyItems();
            }
        }
    }
}
#endif