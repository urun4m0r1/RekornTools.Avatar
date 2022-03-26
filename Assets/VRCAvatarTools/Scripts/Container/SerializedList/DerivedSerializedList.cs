using System;
using UnityEngine;

namespace VRCAvatarTools
{
    #region SerializedList

    [Serializable] public class TextureList : SerializedList<Texture> { }

    [Serializable] public class ShaderPropertiesTables : SerializedList<ShaderPropertiesTable> { }

    [Serializable] public class TexturePresetTables : SerializedList<TexturePresetTable> { }

    #endregion SerializedList

    #region ComponentList

    [Serializable] public class SkinnedMeshRendererList : ComponentList<SkinnedMeshRenderer> { }

    [Serializable] public class RendererList : ComponentList<Renderer> { }

    [Serializable] public class TransformList : ComponentList<Transform> { }

    #endregion ComponentList
}
