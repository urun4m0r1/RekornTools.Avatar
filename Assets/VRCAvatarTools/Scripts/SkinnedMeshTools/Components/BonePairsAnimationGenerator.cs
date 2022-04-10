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
        private const     string ClassName = nameof(BoneFinder);
        [NotNull] private string GameObjectName => gameObject.name;
        [NotNull] private string Header         => $"[{ClassName}({GameObjectName})]";

        private void ShowDialog(string message)
        {
            Debug.LogWarning($"{Header} {message}");
            EditorUtility.DisplayDialog(
                Header,
                message,
                "Confirm");
        }

        private MeshBonePairs _meshBonePairs;

        private void Awake()
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
