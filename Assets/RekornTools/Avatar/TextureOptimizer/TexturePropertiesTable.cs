using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Texture Properties Table")]
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

            TexturePropertiesMap.MatchDictionaryKey(shaders, x => x.GetTexturePropertyList());
        }

        [Button]
        public void ResetTable()
        {
            TexturePropertiesMap.Clear();
            UpdateTable();
        }
    }
}
