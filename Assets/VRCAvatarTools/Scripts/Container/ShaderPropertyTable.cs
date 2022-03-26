using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable]
    public class ShaderPropertiesTables : SerializedList<ShaderPropertiesTable> { }

    [Serializable]
    public class ShaderPropertiesTable : SerializedKeyValue<Shader, ShaderPropertyList>
    {
        public ShaderPropertiesTable(Shader shader, ShaderPropertyList properties) : base(shader, properties) { }

        public bool IsShaderInitialized => Key == Value?.Shader;

        public void Initialize(Func<Shader, ShaderPropertyList> initializer)
        {
            if (IsShaderInitialized) return;

            Value = initializer(Key);
        }
    }

    [Serializable]
    public class ShaderPropertyList : SerializedList<ShaderProperty>
    {
        public Shader Shader { get; private set; }
        public ShaderPropertyList(Shader shader) => Shader = shader;
    }

    [CreateAssetMenu(menuName = "VRC Avatar Tools/Shader Property Tables")]
    public class ShaderPropertyTable : ScriptableObject
    {
        [SerializeField, ListSpan(false), ListMutable(false)]
        public ShaderPropertiesTables Tables = new ShaderPropertiesTables();

        static readonly Func<Shader, ShaderPropertyList> TexturePropertyInitializer =
            ShaderPropertyExtensions.GetTexturePropertyList;

        [Button]
        public void UpdateShaders()
        {
            Undo.RecordObject(this, nameof(ShaderPropertyTable));
            {
                List<Shader> shaders = ShaderPropertyExtensions.GetUsedShadersInProject();
                var          tables  = new ShaderPropertiesTables();

                foreach (ShaderPropertiesTable table in Tables)
                {
                    if (shaders.Contains(table.Key))
                    {
                        tables.Add(table);
                        shaders.Remove(table.Key);
                    }
                }

                foreach (Shader shader in shaders)
                {
                    var table = new ShaderPropertiesTable(shader, null);
                    table.Initialize(TexturePropertyInitializer);
                    tables.Add(table);
                }

                Tables = tables;
            }
        }

        [Button]
        public void Reset()
        {
            Undo.RecordObject(this, nameof(ShaderPropertyTable));
            {
                Tables.Clear();
                UpdateShaders();
            }
        }
    }
}
