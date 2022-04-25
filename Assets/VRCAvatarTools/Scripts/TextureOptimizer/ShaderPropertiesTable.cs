using System;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable]
    public class ShaderPropertiesPerShaderMap : SerializedKeyValue<Shader, ShaderProperties>
    {
        public ShaderPropertiesPerShaderMap(Shader shader, ShaderProperties properties) : base(shader, properties) { }

        public bool IsShaderInitialized => Key == Value?.Shader;

        public void Initialize(Func<Shader, ShaderProperties> initializer)
        {
            if (IsShaderInitialized) return;

            Value = initializer?.Invoke(Key);
        }
    }

    [Serializable]
    public class ShaderProperties : SerializedList<ShaderProperty>
    {
        public Shader Shader { get; private set; }
        public ShaderProperties(Shader shader) => Shader = shader;
    }
    
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Shader Properties Table")]
    public class ShaderPropertiesTable : ScriptableObject
    {
        [SerializeField, ListSpan(false), ListMutable(false)]
        public ShaderPropertiesPerShaderMaps perShaderMaps = new ShaderPropertiesPerShaderMaps();

        private static readonly Func<Shader, ShaderProperties> TexturePropertyInitializer =
            ShaderPropertyExtensions.GetTexturePropertyList;

        public void UpdateShaders()
        {
            Debug.Log("UpdateShaders");
            var shaders = ShaderPropertyExtensions.GetUsedShadersInProject();
            var tables  = new ShaderPropertiesPerShaderMaps();

            foreach (var table in perShaderMaps)
            {
                if (shaders.Contains(table.Key))
                {
                    tables.Add(table);
                    shaders.Remove(table.Key);
                }
            }

            foreach (var shader in shaders)
            {
                var table = new ShaderPropertiesPerShaderMap(shader, null);
                table.Initialize(TexturePropertyInitializer);
                tables.Add(table);
            }

            perShaderMaps = tables;
        }

        public void ResetItems()
        {
            Debug.Log("ResetItems");
            perShaderMaps.Clear();
            UpdateShaders();
        }
    }
}
