using System;
using JetBrains.Annotations;
using UnityEditor.Presets;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable] public class TypedTexturePresetMap : SerializedKeyValue<TextureType, Preset> { }

    [CreateAssetMenu(menuName = "VRC Avatar Tools/Texture Importer Settings")]
    public class TexturePresetSettings : ScriptableObject
    {
        [SerializeField, ListMutable(false)] [NotNull]
        public TypedTexturePresetMaps Presets = new TypedTexturePresetMaps();

        public void Awake()
        {
            if (Presets.Count != GetEnumLength<TextureType>() - 1)
            {
                Presets.Clear();
                foreach (TextureType type in Enum.GetValues(typeof(TextureType)))
                {
                    if (type == TextureType.None) continue;

                    var presetMap = new TypedTexturePresetMap
                    {
                        Key = type,
                    };

                    Presets.Add(presetMap);
                }
            }
        }

        public void Validate()
        {
            foreach (var preset in Presets)
            {
                if (preset == null || !preset.Value) return;

                if (!IsPresetTypeValid(preset.Value, "UnityEditor.TextureImporter"))
                {
                    preset.Value = null;
                }
            }
        }


        private static bool IsPresetTypeValid([NotNull] Preset preset, [NotNull] string name) =>
            preset.GetPresetType().GetManagedTypeName()?.Equals(name) ?? false;

        private static int GetEnumLength<T>() => Enum.GetValues(typeof(T)).Length;
    }
}
