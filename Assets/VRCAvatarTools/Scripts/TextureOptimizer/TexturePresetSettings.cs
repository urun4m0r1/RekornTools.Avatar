using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Presets;
using UnityEngine;

namespace VRCAvatarTools
{
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Texture Preset Settings")]
    public class TexturePresetSettings : ScriptableObject, IValidate
    {
        [SerializeField, ListMutable(false)] [NotNull]
        public TexturePresetMapByType PresetMap = new TexturePresetMapByType();

        [NotNull] private readonly List<TextureType> _types =
            Enum.GetValues(typeof(TextureType)).Cast<TextureType>().ToList();

        public void OnEnable() => MatchDictionaryType();

        public void OnValidate() => ClearInvalidPreset();

        private void MatchDictionaryType()
        {
            foreach (var type in _types)
            {
                if (type == TextureType.None)
                    continue;

                if (!PresetMap.ContainsKey(type))
                    PresetMap.Add(type, null);
            }

            foreach (var map in PresetMap)
            {
                if (!_types.Contains(map.Key))
                    PresetMap.Remove(map.Key);
            }
        }

        private void ClearInvalidPreset()
        {
            foreach (var type in _types)
            {
                if (type == TextureType.None) continue;
                if (!PresetMap.TryGetValue(type, out var preset)) continue;
                if (preset == null) continue;

                if (!IsPresetTypeValid(preset, "UnityEditor.TextureImporter"))
                    PresetMap[type] = null;
            }
        }

        private static bool IsPresetTypeValid([NotNull] Preset preset, [NotNull] string name) =>
            preset.GetPresetType().GetManagedTypeName()?.Equals(name) ?? false;
    }
}
