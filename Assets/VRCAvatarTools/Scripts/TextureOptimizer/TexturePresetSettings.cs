using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Presets;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable] public class TexturePresetMapByType : SerializedDictionary<TexturePresetByType, TextureType, Preset> { }
    [Serializable] public class TexturePresetByType : SerializedKeyValue<TextureType, Preset> { }

    [CreateAssetMenu(menuName = "VRC Avatar Tools/Texture Importer Settings")]
    public class TexturePresetSettings : ScriptableObject
    {
        [SerializeField, ListMutable(false)] [NotNull]
        public TexturePresetMapByType PresetMap = new TexturePresetMapByType();

        [NotNull] private readonly List<TextureType> _types =
            Enum.GetValues(typeof(TextureType)).Cast<TextureType>().ToList();

        public void OnEnable()
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

        private void OnValidate()
        {
            foreach (var map in PresetMap)
            {
                if (!map.Value)
                    return;

                if (!IsPresetTypeValid(map.Value, "UnityEditor.TextureImporter"))
                    PresetMap[map.Key] = null;
            }
        }

        private static bool IsPresetTypeValid([NotNull] Preset preset, [NotNull] string name) =>
            preset.GetPresetType().GetManagedTypeName()?.Equals(name) ?? false;
    }
}
