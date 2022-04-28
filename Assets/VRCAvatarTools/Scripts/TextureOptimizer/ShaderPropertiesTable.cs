using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace VRCAvatarTools
{
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Shader Properties Table")]
    public class ShaderPropertiesTable : ScriptableObject
    {
        [SerializeField, ListSpan(false), ListMutable(false)] [NotNull]
        public TexturePropertiesMapByShader texturePropertiesMap = new TexturePropertiesMapByShader();


        public void OnEnable() => MatchDictionaryType();

        public void UpdateTable() => MatchDictionaryType();

        public void ResetTable()
        {
            texturePropertiesMap.Clear();
            MatchDictionaryType();
        }

        private void MatchDictionaryType()
        {
            var shaders = ShaderPropertyExtensions.AllUserShadersInProject?.ToList();
            if (shaders == null) return;

            foreach (var shader in shaders)
            {
                if (shader == null) continue;

                if (!texturePropertiesMap.ContainsKey(shader))
                {
                    var value = shader.GetTexturePropertyList();
                    if (value != null) texturePropertiesMap.Add(shader, value);
                }
            }

            foreach (var map in texturePropertiesMap)
            {
                if (map.Key == null) continue;

                if (!shaders.Contains(map.Key))
                    texturePropertiesMap.Remove(map.Key);
            }
        }
    }
}
