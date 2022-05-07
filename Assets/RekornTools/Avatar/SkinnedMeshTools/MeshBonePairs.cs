#if UNITY_EDITOR
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class MeshBonePairs : MonoBehaviour
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
        [field: SerializeField] [NotNull] public SkinnedMeshRenderers Meshes { get; private set; } = new SkinnedMeshRenderers();
        [field: SerializeField] [NotNull] public Transforms           Bones  { get; private set; } = new Transforms();
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local

        public void SelectAll()
        {
            var selection = new Object[] { };

            if (Meshes.TryGetSelections(out var meshSelections))
                selection = selection.Concat(meshSelections).ToArray();
            if (Bones.TryGetSelections(out var boneSelections))
                selection = selection.Concat(boneSelections).ToArray();

            if (selection.Length > 0) Selection.objects = selection;
        }

        public void ClearAll()
        {
            ClearMeshes();
            ClearBones();
        }

        public void DestroyAll()
        {
            DestroyMeshes();
            DestroyBones();
        }

        public void SelectMeshes()  => Meshes.SelectComponents();
        public void SelectBones()   => Bones.SelectComponents();
        public void ClearMeshes()   => this.UndoableAction(() => Meshes.Clear());
        public void ClearBones()    => this.UndoableAction(() => Bones.Clear());
        public void DestroyMeshes() => this.UndoableAction(() => Meshes.DestroyItems());
        public void DestroyBones()  => this.UndoableAction(() => Bones.DestroyItems());
    }
}
#endif
