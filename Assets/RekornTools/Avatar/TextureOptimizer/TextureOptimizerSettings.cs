using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace RekornTools.Avatar
{
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Texture Optimizer Settings")]
    public class TextureOptimizerSettings : ScriptableObject, IValidate
    {
        [SerializeField, ReadOnlyList] [NotNull]
        public TexturePresetMapByType PresetMap = new TexturePresetMapByType();

        [NotNull] readonly List<TextureType> _types =
            Enum.GetValues(typeof(TextureType)).Cast<TextureType>().ToList();

        public void OnEnable() => UpdateTable();

        public void OnValidate() => ClearInvalidPreset();

        void UpdateTable()
        {
            PresetMap.MatchDictionaryKey(_types, x => default);
            PresetMap.Remove(TextureType.Ignore);
        }

        void ClearInvalidPreset()
        {
            foreach (var type in _types)
            {
                if (type == TextureType.Ignore) continue;
                if (!PresetMap.TryGetValue(type, out var preset)) continue;
                if (preset == null) continue;

                if (!IsPresetTypeValid(preset, $"{nameof(UnityEditor)}.{nameof(TextureImporter)}"))
                    PresetMap[type] = null;
            }
        }

        static bool IsPresetTypeValid([NotNull] Preset preset, [NotNull] string name) =>
            preset.GetPresetType().GetManagedTypeName()?.Equals(name) ?? false;
    }
}
