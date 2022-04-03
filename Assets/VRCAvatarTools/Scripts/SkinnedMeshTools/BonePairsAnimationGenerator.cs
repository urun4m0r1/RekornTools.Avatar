#if UNITY_EDITOR
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshBonePairs))]
    public sealed class BonePairsAnimationGenerator : MonoBehaviour
    {
        const     string ClassName = nameof(BoneFinder);
        [NotNull] string GameObjectName => gameObject.name;
        [NotNull] string Header         => $"[{ClassName}({GameObjectName})]";

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
        }

        public void CreatePairsToggleAnimation()  { }
        public void CreatePairsEnableAnimation()  { }
        public void CreatePairsDisableAnimation() { }
        public void CreateMeshesToggleAnimation()  { }
        public void CreateMeshesEnableAnimation()  { }
        public void CreateMeshesDisableAnimation() { }
        public void CreateBonesToggleAnimation()  { }
        public void CreateBonesEnableAnimation()  { }
        public void CreateBonesDisableAnimation() { }
    }
}
#endif
