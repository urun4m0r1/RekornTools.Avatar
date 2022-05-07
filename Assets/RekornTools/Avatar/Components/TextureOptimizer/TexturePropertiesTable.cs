using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace RekornTools.Avatar
{
    [CreateAssetMenu(menuName = "Rekorn Tools/Texture Properties Table")]
    public sealed class TexturePropertiesTable : ScriptableObject
    {
        [SerializeField] [ReadOnlyList] [ItemNotSpan] [NotNull]
        public TexturePropertiesMapByShader TexturePropertiesMap = new TexturePropertiesMapByShader();

        public void OnEnable() => UpdateTable();

        [Button]
        public void UpdateTable()
        {
            var shaders = ShaderPropertyExtensions.AllUserShadersInProject?.ToList();
            if (shaders == null) return;

            TexturePropertiesMap.MatchDictionaryKey(shaders, GetTexturePropertyList);
        }

        [Button]
        public void ResetTable()
        {
            TexturePropertiesMap.Clear();
            UpdateTable();
        }

        [CanBeNull]
        public static TextureProperties GetTexturePropertyList([CanBeNull] Shader shader)
        {
            if (shader == null) return null;

            var count = shader.GetPropertyCount();
            if (count == 0) return null;

            var properties = new TextureProperties();
            for (var i = 0; i < count; i++)
            {
                if (shader.GetPropertyFlags(i) == ShaderPropertyFlags.HideInInspector) continue;
                if (shader.GetPropertyType(i)  != ShaderPropertyType.Texture) continue;
                properties.Add(new TextureProperty(shader, i));
            }

            return properties.Count == 0 ? null : properties;
        }
    }
}
