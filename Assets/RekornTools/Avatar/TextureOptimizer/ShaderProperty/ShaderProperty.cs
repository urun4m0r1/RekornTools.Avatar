using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace RekornTools.Avatar
{
    [Serializable]
    public class TextureProperty : ShaderProperty
    {
        public TextureProperty([NotNull] Shader shader, int index, ShaderPropertyType type, [NotNull] string name) : base(shader, index, type, name) { }

        public TextureProperty([NotNull] Shader shader, int index) : base(shader, index) { }
    }

    [Serializable]
    public class ShaderProperty
    {
        [field: SerializeField] public Shader             Shader      { get; private set; }
        [field: SerializeField] public int                Index       { get; private set; }
        [field: SerializeField] public ShaderPropertyType Type        { get; private set; }
        [field: SerializeField] public string             Name        { get; private set; }
        [field: SerializeField] public TextureType        TextureType { get; set; }

        public ShaderProperty([NotNull] Shader shader, int index, ShaderPropertyType type, [NotNull] string name)
        {
            Shader = shader;
            Index  = index;
            Type   = type;
            Name   = name;

            if (type == ShaderPropertyType.Texture) TextureType = AssumeTextureType(name);
        }

        public ShaderProperty([NotNull] Shader shader, int index) :
            this(shader, index, shader.GetPropertyType(index), shader.GetPropertyName(index) ?? "null") { }

        static TextureType AssumeTextureType([NotNull] string name)
        {
            var token = name.ToLower();

            if (token.Contains("normal")) return TextureType.Normal;
            if (token.Contains("emissi")) return TextureType.Emissive;
            if (token.Contains("sample") || token.Contains("pos")) return TextureType.Sampler;
            if (token.Contains("mask")   || token.Contains("map")) return TextureType.Mask;

            return TextureType.Default;
        }
    }
}
