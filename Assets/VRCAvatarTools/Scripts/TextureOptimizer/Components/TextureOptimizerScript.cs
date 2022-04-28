#if UNITY_EDITOR
using System;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public class TextureOptimizerScript : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform _parent;

        [SerializeField] [NotNull] private Renderers _meshes = new Renderers();

        [Header("Textures")]
        [HorizontalLine]
        [SerializeField] private TexturePropertiesTable _propertiesTable;

        [SerializeField, ListMutable(false), ListSpan(false)] [NotNull]
        private TexturesMapByType _texturesMap = new TexturesMapByType();

        [Header("Optimizer")]
        [HorizontalLine]
        [SerializeField] private TextureOptimizerSettings _optimizerSettings;

        private Transform              _prevParent;
        private TexturePropertiesTable _prevTable;

        private void Awake() => _meshes.Initialize(_parent);

        private void OnValidate()
        {
            if (_prevParent == _parent && _prevTable == _propertiesTable) return;

            _prevParent = _parent;
            _meshes.Initialize(_parent);

            _prevTable = _propertiesTable;
            ResetTexturesMap();
            AssignTexturesMap();
        }

        private void ResetTexturesMap()
        {
            _texturesMap.Clear();
            foreach (var type in Enum.GetValues(typeof(TextureType)).Cast<TextureType>())
            {
                if (type == TextureType.Ignore) continue;
                _texturesMap.Add(type, new Textures());
            }
        }

        private void AssignTexturesMap()
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

        private void AssignTextureProperty([NotNull] ShaderProperty property)
        {
            var type = property.TextureType;
            if (type == TextureType.Ignore) return;
            _texturesMap[type]?.AddRange(GetTextureList(property));
        }

        [CanBeNull] private Textures GetTextureList([NotNull] ShaderProperty property)
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

        private bool TryFindMatchingTexture([CanBeNull] Material material, [NotNull] ShaderProperty property, [CanBeNull] out Texture texture)
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

        private bool IsTextureExist([NotNull] Texture texture) =>
            _texturesMap.Any(x => x.Value?.Contains(texture) == true);

        [Button] private void Optimize() { }
    }
}
#endif
