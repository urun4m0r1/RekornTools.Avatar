using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace RekornTools.Avatar
{
    readonly struct AssetSize
    {
        [CanBeNull] string RuntimeBytes => FormatBytes(Runtime);
        [CanBeNull] string StorageBytes => FormatBytes(Storage);

        [CanBeNull] static string FormatBytes(long size)
        {
            var absoluteSize = size < 0L ? -size : size;
            return size < 0L ? $"-{EditorUtility.FormatBytes(absoluteSize)}" : EditorUtility.FormatBytes(absoluteSize);
        }

        long Runtime { get; }
        long Storage { get; }

        public AssetSize(long runtime, long storage)
        {
            Runtime = runtime;
            Storage = storage;
        }

        public override string ToString() => $"{RuntimeBytes} (Runtime) / {StorageBytes} (Storage)";

        public static AssetSize operator +(AssetSize left, AssetSize right) =>
            new AssetSize(left.Runtime + right.Runtime, left.Storage + right.Storage);

        public static AssetSize operator -(AssetSize left, AssetSize right) =>
            new AssetSize(left.Runtime - right.Runtime, left.Storage - right.Storage);


        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public static AssetSize GetAssetSize([CanBeNull] Texture texture)
        {
            if (texture == null) return new AssetSize(0L, 0L);

            var type       = Assembly.Load("UnityEditor.dll")?.GetType("UnityEditor.TextureUtil");
            var methodInfo = type?.GetMethod("GetStorageMemorySizeLong", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

            var runtime = Profiler.GetRuntimeMemorySizeLong(texture);
            var storage = (long)(methodInfo?.Invoke(null, new object[] { texture }) ?? 0L);

            return new AssetSize(runtime, storage);
        }
    }
}
