#if UNITY_EDITOR
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class AssetManager : MonoBehaviour, IValidate
    {
        [SerializeField] public Transform Parent;

        [SerializeField] [ReadOnlyList] [NotNull] public Renderers Renderers = new Renderers();
        [SerializeField] [ReadOnlyList] [NotNull] public Materials Materials = new Materials();
        [SerializeField] [ReadOnlyList] [NotNull] public Shaders   Shaders   = new Shaders();
        [SerializeField] [ReadOnlyList] [NotNull] public Textures  Textures  = new Textures();

        [SerializeField] [HideInInspector] Transform _prevParent;

        void Awake() => Refresh();

        public void OnValidate()
        {
            if (_prevParent != Parent) Refresh();
        }

        [Button]
        public void Refresh()
        {
            _prevParent = Parent;
            Renderers.Initialize(Parent);

            Materials.Clear();
            Shaders.Clear();
            Textures.Clear();

            foreach (var (material, shader) in
                     from r in Renderers
                     from material in r.sharedMaterials
                     select (material, material.shader))
            {
                if (!Materials.Contains(material)) Materials.Add(material);
                if (!Shaders.Contains(shader)) Shaders.Add(shader);

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
                if (texture != null && !Textures.Contains(texture)) Textures.Add(texture);
            }
        }
    }
}
#endif
