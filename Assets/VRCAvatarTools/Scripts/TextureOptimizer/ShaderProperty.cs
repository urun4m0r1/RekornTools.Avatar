using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    [Serializable]
    public class ShaderProperty
    {
        [NotNull] public const string ShaderField      = nameof(Shader);
        [NotNull] public const string IndexField       = nameof(Index);
        [NotNull] public const string NameField        = nameof(Name);
        [NotNull] public const string TypeField        = nameof(Type);
        [NotNull] public const string TextureTypeField = nameof(TextureType);

        [field: SerializeField] public Shader             Shader      { get; private set; }
        [field: SerializeField] public int                Index       { get; private set; }
        [field: SerializeField] public string             Name        { get; private set; }
        [field: SerializeField] public ShaderPropertyType Type        { get; private set; }
        [field: SerializeField] public TextureType        TextureType { get; set; }

        public ShaderProperty(
            [NotNull] Shader   shader,
            int                index,
            [NotNull] string   name,
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
            var token = name.ToLower();

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
