#if UNITY_EDITOR
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class AssetManager : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] Transform _parent;

        [Header("Renderers")]
        [SerializeField] [ReadOnlyList] [NotNull] Renderers _renderers = new Renderers();

        [Header("Materials")]
        [SerializeField] [ReadOnlyList] [NotNull] Materials _materials = new Materials();

        [Header("Shaders")]
        [SerializeField] [ReadOnlyList] [NotNull] Shaders _shaders = new Shaders();

        [Header("Textures")]
        [SerializeField] [ReadOnlyList] [NotNull] Textures _textures = new Textures();

        [SerializeField] [HideInInspector] Transform _prevParent;

        void Awake() => _renderers.Initialize(_parent);

        void OnValidate()
        {
            if (_prevParent != _parent) Refresh();
        }

        [Button]
        void Refresh()
        {
            _prevParent = _parent;
            _renderers.Initialize(_parent);

            _materials.Clear();
            _shaders.Clear();
            _textures.Clear();

            foreach (var (material, shader) in
                     from r in _renderers
                     from material in r.sharedMaterials
                     select (material, material.shader))
            {
                if (!_materials.Contains(material)) _materials.Add(material);
                if (!_shaders.Contains(shader)) _shaders.Add(shader);

                AppendTexturesList(material, shader);
            }
        }

        void AppendTexturesList([CanBeNull] Material material, [CanBeNull] Shader shader)
        {
            if (material == null || shader == null) return;
            for (var i = 0; i < ShaderUtil.GetPropertyCount(shader); i++)
            {
                if (ShaderUtil.GetPropertyType(shader, i) != ShaderUtil.ShaderPropertyType.TexEnv) continue;
                var texture = material.GetTexture(ShaderUtil.GetPropertyName(shader, i));
                if (texture != null && !_textures.Contains(texture)) _textures.Add(texture);
            }
        }
    }
}
#endif
