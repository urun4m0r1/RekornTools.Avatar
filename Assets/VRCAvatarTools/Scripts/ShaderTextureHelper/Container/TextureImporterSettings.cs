using System;
using UnityEditor.Presets;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable]
    public class TexturePresetTable
    {
        public static string TypeFieldName   => nameof(Type);
        public static string PresetFieldName => nameof(Preset);

        [field: SerializeField] public TextureType Type   { get; private set; }
        [field: SerializeField] public Preset      Preset { get; private set; }

        public TexturePresetTable(TextureType type) => Type = type;

        public void ValidatePreset()
        {
            if (!Preset) return;

            if (!Preset.GetPresetType().GetManagedTypeName().Equals("UnityEditor.TextureImporter"))
                Preset = null;
        }
    }

    [CreateAssetMenu(menuName = "VRC Avatar Tools/Texture Importer Settings")]
    public class TextureImporterSettings : ScriptableObject
    {
        [SerializeField, ListMutable(false)] public TexturePresetTables Presets = new TexturePresetTables();

        public void OnValidate()
        {
            if (Presets.Count != Enum.GetValues(typeof(TextureType)).Length - 1)
            {
                Presets.Clear();
                foreach (TextureType type in Enum.GetValues(typeof(TextureType)))
                {
                    if (type == TextureType.None) continue;

                    Presets.Add(new TexturePresetTable(type));
                }
            }

            foreach (TexturePresetTable preset in Presets) preset.ValidatePreset();
        }
    }
}
