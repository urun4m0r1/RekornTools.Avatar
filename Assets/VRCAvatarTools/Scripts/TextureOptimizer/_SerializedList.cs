using System;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable] public class TypedTexturesMaps : SerializedList<TypedTexturesMap> { }

    [Serializable] public class TypedTexturesMap : SerializedKeyValue<TextureType, Textures> { }

    [Serializable] public class Textures : SerializedList<Texture> { }

    [Serializable] public class Renderers : ComponentList<Renderer> { }

    [Serializable] public class ShaderPropertiesPerShaderMaps : SerializedList<ShaderPropertiesPerShaderMap> { }
}
