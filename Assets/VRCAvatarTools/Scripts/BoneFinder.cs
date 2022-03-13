#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public sealed class BoneFinder : MonoBehaviour
    {
        [field: SerializeField, Label(nameof(Target))] public Transform               Target { get; private set; }
        [field: SerializeField, Label(nameof(Name))]   public string                  Name   { get; private set; }
        [field: SerializeField, Label(nameof(Meshes))] public SkinnedMeshRendererList Meshes { get; private set; }
        [field: SerializeField, Label(nameof(Bones))]  public TransformList           Bones  { get; private set; }

        const     string ClassName = nameof(BoneFinder);
        [NotNull] string GameObjectName => gameObject.name;
        [NotNull] string Header         => $"[{ClassName}({GameObjectName})]";

        void Log(string        message) => Debug.Log($"{Header} {message}");
        void LogWarning(string message) => Debug.LogWarning($"{Header} {message}");
        void LogError(string   message) => Debug.LogError($"{Header} {message}");

        [Button] void DestroyAll()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Meshes.DestroyItems();
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.DestroyItems();
        }

        [Button] public void SelectAll()
        {
            if (Meshes.TryGetSelections(out Object[] meshSelections)
                && Bones.TryGetSelections(out Object[] boneSelections))
                Selection.objects = meshSelections.Concat(boneSelections).ToArray();
        }

        [Button] public void InitializeFromTargetChildren()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Meshes.InitializeFromName(Target);
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.InitializeFromName(Target);

            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.RemoveRange(Meshes.Select(x => x.transform).ToList());
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.Remove(Target);
        }

        [Button] public void InitializeFromTargetChildrenWithName()
        {
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Meshes.InitializeFromName(Target, Name);
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.InitializeFromName(Target, Name);

            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.RemoveRange(Meshes.Select(x => x.transform).ToList());
            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.Remove(Target);
        }

        [Button] void FindChildrenWithMeshes()
        {
            FindBonesWithMeshes();

            var children = new List<Transform>();

            foreach (Transform child in
                     from b in Bones
                     from Transform c in b
                     where c && !Bones.Contains(c) && !children.Contains(c)
                     select c)
            {
                children.Add(child);
            }

            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.AddRange(children);
        }

        [Button] void FindBonesWithMeshes()
        {
            if (Meshes.Count == 0)
            {
                LogWarning("There are no meshes to find bones from.");
                return;
            }

            Undo.RegisterCompleteObjectUndo(this, ClassName);
            Bones.Clear();

            foreach (Transform bone in
                     from m in Meshes
                     where m
                     from b in m.bones
                     where b && !Bones.Contains(b)
                     select b)
            {
                Undo.RegisterCompleteObjectUndo(this, ClassName);
                Bones.Add(bone);
            }

            if (Bones.Count == 0)
            {
                LogWarning("Failed to find any bones from meshes.");
                Log("You might need to check meshes is valid or bone weights are not set to zero.");
            }
        }
    }
}
#endif
