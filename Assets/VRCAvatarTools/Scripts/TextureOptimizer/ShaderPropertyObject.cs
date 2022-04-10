using System;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable]
    public class ShaderPropertiesTable : SerializedKeyValue<Shader, ShaderPropertyList>
    {
        public ShaderPropertiesTable(Shader shader, ShaderPropertyList properties) : base(shader, properties) { }

        public bool IsShaderInitialized => Key == Value?.Shader;

        public void Initialize(Func<Shader, ShaderPropertyList> initializer)
        {
            if (IsShaderInitialized) return;

            Value = initializer?.Invoke(Key);
        }
    }

    [Serializable]
    public class ShaderPropertyList : SerializedList<ShaderProperty>
    {
        public Shader Shader { get; private set; }
        public ShaderPropertyList(Shader shader) => Shader = shader;
    }

    public class ShaderPropertyObject : ScriptableObject
    {
        [SerializeField, ListSpan(false), ListMutable(false)]
        public ShaderPropertiesTables Tables = new ShaderPropertiesTables();

        private static readonly Func<Shader, ShaderPropertyList> TexturePropertyInitializer =
            ShaderPropertyExtensions.GetTexturePropertyList;

        public void UpdateShaders()
        {
            Debug.Log("UpdateShaders");
            var shaders = ShaderPropertyExtensions.GetUsedShadersInProject();
            var tables  = new ShaderPropertiesTables();

            foreach (var table in Tables)
            {
                if (shaders.Contains(table.Key))
                {
                    tables.Add(table);
                    shaders.Remove(table.Key);
                }
            }

            foreach (var shader in shaders)
            {
                var table = new ShaderPropertiesTable(shader, null);
                table.Initialize(TexturePropertyInitializer);
                tables.Add(table);
            }

            Tables = tables;
        }

        public void ResetItems()
        {
            Debug.Log("ResetItems");
            Tables.Clear();
            UpdateShaders();
        }
    }
}
