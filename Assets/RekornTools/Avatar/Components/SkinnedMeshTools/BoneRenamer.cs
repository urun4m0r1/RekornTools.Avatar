#if UNITY_EDITOR
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class BoneRenamer : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] Transform _parent;
        [SerializeField] RigNamingConvention _sourceNaming = RigNamingConvention.Default;

        [Header("Target")]
        [SerializeField] RigNamingConvention _targetNaming = RigNamingConvention.Default;

        [Header("Rename Token")]
        [SerializeField] [NotNull] NamePairs _renameToken = new NamePairs();

        [Header("Rename Exclusions")]
        [SerializeField] [NotNull] Transforms _exclusions = new Transforms();

        [Button]
        public void Rename()
        {
            if (_parent == null) return;

            _parent.InvokeRecursive(x =>
            {
                if (x == null || _exclusions.Contains(x)) return;

                var newName = x.name;

                foreach (var token in _renameToken)
                {
                    if (string.IsNullOrWhiteSpace(token?.Source)) continue;
                    newName = newName.Replace(token.Source, token.Target);
                }

                newName = RigNamingConvention.Convert(newName, _sourceNaming, _targetNaming);

                x.gameObject.UndoableAction(() => x.name = newName);
            });
        }
    }
}
#endif
