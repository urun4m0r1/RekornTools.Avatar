#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public sealed class MeshBonePairs : MonoBehaviour
    {
        [field: SerializeField]
        public SkinnedMeshRendererList Meshes { get; private set; } = new SkinnedMeshRendererList();

        [field: SerializeField]
        public TransformList Bones { get; private set; } = new TransformList();

        const string ClassName = nameof(BoneFinder);

        public void SelectAll()
        {
            Object[] selection = { };

            if (Meshes.TryGetSelections(out Object[] meshSelections))
                selection = selection.Concat(meshSelections).ToArray();
            if (Bones.TryGetSelections(out Object[] boneSelections))
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
