using System;
using UnityEditor.Presets;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable]
    public class TypedTexturePresetMap : SerializedKeyValue<TextureType, Preset>
    {
        public TypedTexturePresetMap(TextureType type) => Key = type;

        public void ValidatePreset()
        {
            if (!Value) return;

            if (!Value.GetPresetType().GetManagedTypeName().Equals("UnityEditor.TextureImporter"))
                Value = null;
        }
    }

    [CreateAssetMenu(menuName = "VRC Avatar Tools/Texture Importer Settings")]
    public class TexturePresetSettings : ScriptableObject
    {
        [SerializeField, ListMutable(false)] public TypedTexturePresetMaps Presets = new TypedTexturePresetMaps();

        public void OnValidate()
        {
            if (Presets.Count != Enum.GetValues(typeof(TextureType)).Length - 1)
            {
                Presets.Clear();
                foreach (TextureType type in Enum.GetValues(typeof(TextureType)))
                {
                    if (type == TextureType.None) continue;

                    Presets.Add(new TypedTexturePresetMap(type));
                }
            }

            foreach (var preset in Presets) preset.ValidatePreset();
        }
    }
}
