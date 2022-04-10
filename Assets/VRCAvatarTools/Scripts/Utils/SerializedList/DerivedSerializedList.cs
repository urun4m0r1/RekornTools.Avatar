using System;
using UnityEngine;

namespace VRCAvatarTools
{
#region SerializedList
    [Serializable] public class TexturePropertyMapList : SerializedList<TexturePropertyMap> { }

    [Serializable] public class TextureList : SerializedList<Texture> { }

#endregion SerializedList

#region ComponentList
    [Serializable] public class SkinnedMeshRendererList : ComponentList<SkinnedMeshRenderer> { }

    [Serializable] public class RendererList : ComponentList<Renderer> { }

    [Serializable] public class TransformList : ComponentList<Transform> { }
#endregion ComponentList

    [Serializable] public class TexturePropertyMap : SerializedKeyValue<TextureType, TextureList> { }
}
