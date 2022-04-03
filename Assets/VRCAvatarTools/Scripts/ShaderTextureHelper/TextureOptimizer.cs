using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{

    [ExecuteInEditMode]
    public class TextureOptimizer : MonoBehaviour
    {
        [SerializeField] TextureImporterSettings settings;
        [SerializeField] ShaderPropertyTable     shaderPropertyTable;
        [SerializeField] Transform               parent;
        [SerializeField] RendererList            meshes = new RendererList();

        [SerializeField, ListMutable(false), ListSpan(false)] TexturePropertyMapList texturePropertyMapList = new TexturePropertyMapList();

        Transform _prevParent;

        void Awake()
        {
            meshes.Initialize(parent);
        }

        void OnValidate()
        {
            if (_prevParent != parent)
            {
                _prevParent = parent;
                meshes.Initialize(parent);

                texturePropertyMapList.Clear();

                foreach (var type in Enum.GetValues(typeof(TextureType)))
                {
                    if ((TextureType)type == TextureType.None) continue;
                    var map = new TexturePropertyMap
                    {
                        Key = (TextureType)type,
                        Value = new TextureList(),
                    };
                    texturePropertyMapList.Add(map);
                }

                foreach (var table in shaderPropertyTable.Tables)
                {
                    if (table.Value == null) continue;
                    foreach (var property in table.Value)
                    {
                        if (property.Type != ShaderPropertyType.Texture) continue;
                        if (property.TextureType == TextureType.None) continue;

                        foreach (TexturePropertyMap t in texturePropertyMapList)
                        {
                            if (t.Key == property.TextureType)
                            {
                                var textureList = GetTextureList(property);
                                if (textureList?.Count == 0) continue;

                                t.Value.AddRange(textureList);
                            }
                        }
                    }
                }
            }
        }

        List<Texture> GetTextureList(ShaderProperty property)
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
                    if (texturePropertyMapList.Count(t => t.Value?.Contains(texture) == true) > 0) continue;

                    list.Add(texture);
                }
            }

            return list;
        }

        [Button] void Optimize() { }
    }
}
