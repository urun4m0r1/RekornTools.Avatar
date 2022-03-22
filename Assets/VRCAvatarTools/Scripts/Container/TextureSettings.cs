using System;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable]
    public class TextureOptionsTables : SerializedList<TextureOptionsTable> { }

    [Serializable]
    public class TextureOptionsTable : SerializedKeyValuePair<TextureType, TextureOptions> { }

    [Serializable]
    public class TextureOptions
    {
        [field: SerializeField] public int Size { get; private set; } = 512;

        [field: SerializeField] public TextureImporterCompression CompressType { get; private set; } =
            TextureImporterCompression.CompressedLQ;

        [field: SerializeField] public bool IsCrunchCompress { get; private set; } = true;

        [field: SerializeField, Range(0, 100), ShowIf(nameof(IsCrunchCompress)), AllowNesting]
        public int CrunchQuality { get; private set; } = 50;
    }

    [CreateAssetMenu(menuName = "VRCAvatarTools/TextureSettings")]
    public class TextureSettings : ScriptableObject
    {
        [SerializeField, ListSpan(false)] public TextureOptionsTables Tables = new TextureOptionsTables();
    }
}
