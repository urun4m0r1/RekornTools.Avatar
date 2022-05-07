using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;
using UnityEditor.Presets;

namespace RekornTools.Avatar
{
    public static class AssetManager
    {
        public static void ApplyPreset([CanBeNull] Object obj, [NotNull] Preset preset)
        {
            var importer = GetAssetImporter(obj);
            if (importer == null) return;

            preset.ApplyTo(importer);
            importer.SaveAndReimport();
        }

        [CanBeNull]
        public static AssetImporter GetAssetImporter([CanBeNull] Object obj)
        {
            if (obj == null) return null;

            var path = AssetDatabase.GetAssetPath(obj);
            return AssetImporter.GetAtPath(path);
        }
    }
}
