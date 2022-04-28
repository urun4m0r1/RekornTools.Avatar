using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace VRCAvatarTools
{
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Shader Properties Table")]
    public class ShaderPropertiesTable : ScriptableObject
    {
        [SerializeField, ListSpan(false), ListMutable(false)] [NotNull]
        public ShaderPropertiesMapByShader ShaderPropertiesMap = new ShaderPropertiesMapByShader();

        [NotNull] private static List<Shader> ProjectShaders => ShaderPropertyExtensions.GetUsedShadersInProject();

        public void ResetItems()
        {
            ShaderPropertiesMap.Clear();
            MatchDictionaryType();
        }

        public void OnEnable() => MatchDictionaryType();

        private void MatchDictionaryType()
        {
            foreach (var shader in ProjectShaders)
            {
                if (shader == null) continue;

                if (!ShaderPropertiesMap.ContainsKey(shader))
                    ShaderPropertiesMap.Add(shader, shader.GetTexturePropertyList());
            }

            foreach (var map in ShaderPropertiesMap)
            {
                if (map.Key == null) continue;

                if (!ProjectShaders.Contains(map.Key))
                    ShaderPropertiesMap.Remove(map.Key);
            }
        }
    }
}
