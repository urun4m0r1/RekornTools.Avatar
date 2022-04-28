using System;
using UnityEditor.Presets;
using UnityEngine;

namespace VRCAvatarTools
{
#region List
    [Serializable] public class Textures : SerializedList<Texture> { }

    [Serializable] public class Renderers : ComponentList<Renderer> { }

    [Serializable] public class TextureProperties : SerializedList<TextureProperty> { }

    [Serializable] public class ShaderProperties : SerializedList<ShaderProperty> { }
#endregion // List

#region Dictionary
    [Serializable] public class TexturePresetMapByType : SerializedDictionary<TexturePresetByType, TextureType, Preset> { }

    [Serializable] public class TexturesMapByType : SerializedDictionary<TexturesByType, TextureType, Textures> { }

    [Serializable] public class TexturePropertiesMapByShader : SerializedDictionary<TexturePropertiesByShader, Shader, TextureProperties> { }
#endregion // Dictionary

#region KeyValue
    [Serializable] public class TexturePresetByType : SerializedKeyValue<TextureType, Preset> { }

    [Serializable] public class TexturesByType : SerializedKeyValue<TextureType, Textures> { }

    [Serializable] public class TexturePropertiesByShader : SerializedKeyValue<Shader, TextureProperties> { }
#endregion // KeyValue
}
