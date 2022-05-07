using System;
using UnityEditor.Presets;
using UnityEngine;

namespace RekornTools.Avatar
{
#region List
    [Serializable] public sealed class Textures : SerializedList<Texture> { }

    [Serializable] public sealed class Renderers : ComponentList<Renderer> { }

    [Serializable] public sealed class TextureProperties : SerializedList<TextureProperty> { }

    [Serializable] public sealed class ShaderProperties : SerializedList<ShaderProperty> { }
#endregion // List

#region Dictionary
    [Serializable] public sealed class TexturePresetMapByType : SerializedDictionary<TexturePresetByType, TextureType, Preset> { }

    [Serializable] public sealed class TexturesMapByType : SerializedDictionary<TexturesByType, TextureType, Textures> { }

    [Serializable] public sealed class TexturePropertiesMapByShader : SerializedDictionary<TexturePropertiesByShader, Shader, TextureProperties> { }
#endregion // Dictionary

#region KeyValue
    [Serializable] public sealed class TexturePresetByType : HorizontalKeyValue<TextureType, Preset> { }

    [Serializable] public sealed class TexturesByType : HorizontalKeyValue<TextureType, Textures> { }

    [Serializable] public sealed class TexturePropertiesByShader : SerializedKeyValue<Shader, TextureProperties> { }
#endregion // KeyValue
}
