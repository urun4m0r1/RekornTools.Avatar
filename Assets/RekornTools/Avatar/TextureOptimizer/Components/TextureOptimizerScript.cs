#if UNITY_EDITOR
using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public class TextureOptimizerScript : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField]
        Transform _parent;

        [SerializeField] [NotNull] Renderers _meshes = new Renderers();

        [Header("Textures")]
        [SerializeField]
        TexturePropertiesTable _propertiesTable;

        [SerializeField, ReadOnlyList, ItemNotSpan] [NotNull] TexturesMapByType _texturesMap = new TexturesMapByType();

        [Header("Optimizer")]
        [SerializeField]
        TextureOptimizerSettings _optimizerSettings;

        Transform              _prevParent;
        TexturePropertiesTable _prevTable;

        void Awake() => _meshes.Initialize(_parent);

        void OnValidate()
        {
            if (_prevParent == _parent && _prevTable == _propertiesTable) return;

            _prevParent = _parent;
            _meshes.Initialize(_parent);

            _prevTable = _propertiesTable;
            ResetTexturesMap();
            AssignTexturesMap();
        }

        void ResetTexturesMap()
        {
            _texturesMap.Clear();
            foreach (var type in Enum.GetValues(typeof(TextureType)).Cast<TextureType>())
            {
                if (type == TextureType.Ignore) continue;
                _texturesMap.Add(type, new Textures());
            }
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
            if (_optimizerSettings == null) return;
            var presets = _optimizerSettings.PresetMap;
            foreach (var map in _texturesMap)
            {
                var textures = map.Value;
                if (textures == null || textures.Count == 0) continue;

                if (!presets.TryGetValue(map.Key, out var preset)) continue;
                if (preset == null) continue;

                foreach (var texture in textures)
                {
                    var path     = AssetDatabase.GetAssetPath(texture);
                    var importer = AssetImporter.GetAtPath(path);
                    preset.ApplyTo(importer);
                }
            }

            //TODO: 절약한 용량 표시
        }
    }
}
#endif
