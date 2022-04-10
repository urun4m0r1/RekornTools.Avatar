using System;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable] public class TexturePropertyMapList : SerializedList<TexturePropertyMap> { }

    [Serializable] public class TextureList : SerializedList<Texture> { }

    [Serializable] public class TexturePropertyMap : SerializedKeyValue<TextureType, TextureList> { }

    [Serializable] public class RendererList : ComponentList<Renderer> { }
}
