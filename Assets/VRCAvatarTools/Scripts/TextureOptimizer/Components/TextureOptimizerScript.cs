#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public class TextureOptimizerScript : MonoBehaviour
    {
        [SerializeField] private TexturePresetSettings settings;
        [SerializeField] private Transform             parent;

        [SerializeField] [NotNull] private Renderers meshes = new Renderers();

        [SerializeField, ListMutable(false), ListSpan(false)] [NotNull]
        private TexturesMapByType texturesMap = new TexturesMapByType();

        private Transform _prevParent;

        private void Awake()
        {
            meshes.Initialize(parent);
        }

        private void OnValidate()
        {
            if (_prevParent != parent)
            {
                _prevParent = parent;
                meshes.Initialize(parent);

                texturesMap.Clear();

                foreach (var type in Enum.GetValues(typeof(TextureType)))
                {
                    if ((TextureType)type == TextureType.None) continue;
                    texturesMap.Add((TextureType)type, new Textures());
                }

                // foreach (var table in textureTypeListMapList)
                // {
                //     if (table.Value == null) continue;
                //     foreach (var property in table.Value)
                //     {
                //         if (property.Type != ShaderPropertyType.Texture) continue;
                //         if (property.TextureType == TextureType.None) continue;
                //
                //         foreach (var t in texturePropertyMapList)
                //         {
                //             if (t.Key == property.TextureType)
                //             {
                //                 var textureList = GetTextureList(property);
                //                 if (textureList?.Count == 0) continue;
                //
                //                 t.Value.AddRange(textureList);
                //             }
                //         }
                //     }
                // }
            }
        }

        private List<Texture> GetTextureList(ShaderProperty property)
        {
            var list = new List<Texture>();
            foreach (var r in meshes)
            {
                var material = r.sharedMaterial;
                if (!material) continue;
                var shader = material.shader;
                if (!shader) continue;

                if (property.Shader != shader) continue;

                if (material.HasProperty(property.Name))
                {
                    var texture = material.GetTexture(property.Name);
                    if (!texture) continue;
                    if (list.Contains(texture)) continue;
                    if (texturesMap.Count(t => t.Value?.Contains(texture) == true) > 0) continue;

                    list.Add(texture);
                }
            }

            return list;
        }

        [Button] private void Optimize() { }
    }
}
#endif
