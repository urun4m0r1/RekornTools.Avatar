using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    [Serializable]
    public class ShaderProperty
    {
        public static readonly string ShaderField      = nameof(Shader);
        public static readonly string IndexField       = nameof(Index);
        public static readonly string NameField        = nameof(Name);
        public static readonly string TypeField        = nameof(Type);
        public static readonly string TextureTypeField = nameof(TextureType);

        [field: SerializeField] public Shader             Shader      { get; private set; }
        [field: SerializeField] public int                Index       { get; private set; }
        [field: SerializeField] public string             Name        { get; private set; }
        [field: SerializeField] public ShaderPropertyType Type        { get; private set; }
        [field: SerializeField] public TextureType        TextureType { get; set; }

        public ShaderProperty(
            Shader             shader,
            int                index,
            string             name,
            ShaderPropertyType type)
        {
            Shader = shader;
            Index  = index;
            Name   = name;
            Type   = type;

            if (type == ShaderPropertyType.Texture)
                TextureType = AssumeTextureType(name);
        }

        static TextureType AssumeTextureType([NotNull] string name)
        {
            string token = name.ToLower();

            if (token.Contains("normal"))
                return TextureType.Normal;

            if (token.Contains("emissi"))
                return TextureType.Emissive;

            if (token.Contains("sample") || token.Contains("pos"))
                return TextureType.Sampler;

            if (token.Contains("mask") || token.Contains("map"))
                return TextureType.Mask;

            return TextureType.Default;
        }
    }
}