#if UNITY_EDITOR
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class TextureOptimizer : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] Transform _parent;
        [SerializeField] [NotNull] Renderers _meshes = new Renderers();

        [Header("Textures")]
        [SerializeField] TexturePropertiesTable _propertiesTable;
        [SerializeField] [ReadOnlyList] [ItemNotSpan] [NotNull] TexturesMapByType _texturesMap = new TexturesMapByType();

        [Header("Optimizer")]
        [SerializeField] TextureOptimizerSettings _optimizerSettings;

        [SerializeField] [HideInInspector] Transform              _prevParent;
        [SerializeField] [HideInInspector] TexturePropertiesTable _prevTable;

        void Awake() => _meshes.Initialize(_parent);

        void OnValidate()
        {
            if (_prevParent != _parent || _prevTable != _propertiesTable) Refresh();
        }

        void Refresh()
        {
            _prevParent = _parent;
            _meshes.Initialize(_parent);

            _prevTable = _propertiesTable;
            ResetTexturesMap();
            AssignTexturesMap();
        }

        void ResetTexturesMap()
        {
            _texturesMap.Clear();
            _texturesMap.MatchDictionaryKey(_ => new Textures());
            _texturesMap.Remove(TextureType.Ignore);
        }

        void AssignTexturesMap()
        {
            if (_propertiesTable == null) return;
            foreach (var properties in _propertiesTable.TexturePropertiesMap.Values)
            {
                if (properties == null) continue;
                foreach (var property in properties)
                {
                    if (property == null) continue;
                    AssignTextureProperty(property);
                }
            }
        }

        void AssignTextureProperty([NotNull] ShaderProperty property)
        {
            var type = property.TextureType;
            if (type == TextureType.Ignore) return;
            _texturesMap[type]?.AddRange(GetTextureList(property));
        }

        [CanBeNull]
        Textures GetTextureList([NotNull] ShaderProperty property)
        {
            var list = new Textures();
            foreach (var mesh in _meshes)
            {
                if (mesh == null) continue;
                AddTextures(mesh.sharedMaterials);
            }

            return list.Count == 0 ? null : list;

            void AddTextures(Material[] materials)
            {
                if (materials == null) return;
                foreach (var material in materials)
                    if (TryFindMatchingTexture(material, property, out var texture))
                        AppendTexture(texture);
            }

            void AppendTexture(Texture texture)
            {
                if (!list.Contains(texture)) list.Add(texture);
            }
        }

        bool TryFindMatchingTexture([CanBeNull] Material material, [NotNull] ShaderProperty property, [CanBeNull] out Texture texture)
        {
            texture = null;
            if (material        == null) return false;
            if (material.shader != property.Shader) return false;
            if (!material.HasProperty(property.Name)) return false;

            texture = material.GetTexture(property.Name);
            if (texture == null) return false;
            if (IsTextureExist(texture)) return false;

            return true;
        }

        bool IsTextureExist([NotNull] Texture texture) =>
            _texturesMap.Any(x => x.Value?.Contains(texture) == true);

        [Button]
        void Optimize()
        {
            if (_propertiesTable != null) _propertiesTable.UpdateTable();

            if (_optimizerSettings == null)
            {
                this.ShowConfirmDialog("You need to set the optimizer settings first.");
                return;
            }

            if (!EditorUtility.DisplayDialog("Warning"
                                           , "This operation will reimport all textures from list. "
                                           + "This operation can't be undone, and takes huge amount of time."
                                           , "Proceed"
                                           , "Abort")) return;

            ApplyPreset(_texturesMap, _optimizerSettings.PresetMap);
        }

        void ApplyPreset([NotNull] TexturesMapByType texturesMapByType, [NotNull] TexturePresetMapByType presets)
        {
            var count    = 0;
            var prevSize = new AssetSize(0L, 0L);
            var newSize  = new AssetSize(0L, 0L);

            foreach (var map in texturesMapByType)
            {
                if (!presets.TryGetValue(map.Key, out var preset)) continue;
                if (preset    == null) continue;
                if (map.Value == null) continue;

                foreach (var texture in map.Value)
                {
                    count++;
                    prevSize += AssetSize.GetAssetSize(texture);
                    AssetHelper.ApplyPreset(texture, preset);
                    newSize += AssetSize.GetAssetSize(texture);
                }
            }

            var savedSize = newSize - prevSize;
            this.ShowConfirmDialog($"Total {count} textures optimized.\n"
                                 + $"[Before] {prevSize.ToString()}\n"
                                 + $"[After] {newSize.ToString()}\n"
                                 + $"[Saved] {savedSize.ToString()}\n");
        }
    }
}
#endif
