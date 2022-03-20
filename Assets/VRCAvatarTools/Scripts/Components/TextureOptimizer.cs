using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace VRCAvatarTools
{
    public enum TextureType
    {
        Opaque,
        Alpha,
        Emissive,
        Normal,
        Mask,
    }

    [Serializable]
    public class TextureList : ObjectList<Texture> { }

    [Serializable]
    public class CustomTextureList
    {
        [SerializeField] public TextureType Type;
        [SerializeField] public Shader      Shader;
        [SerializeField] public string      Property;
        [SerializeField] public TextureList List = new TextureList();
    }

    [ExecuteInEditMode]
    public class TextureOptimizer : MonoBehaviour
    {
        [SerializeField] Transform    parent;
        [SerializeField] RendererList meshes = new RendererList();

        [SerializeField, ReorderableList] List<CustomTextureList> textures = new List<CustomTextureList>();

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
            }

            foreach (CustomTextureList list in textures)
            {
                list.List.Clear();

                foreach (Renderer r in meshes)
                {
                    foreach (Material m in r.sharedMaterials)
                    {
                        if (m.shader != list.Shader) continue;

                        int id = Shader.PropertyToID(list.Property);
                        if (!m.HasProperty(id)) continue;

                        Texture tex = m.GetTexture(id);
                        if (tex == null) continue;

                        if (list.List.Contains(tex)) continue;

                        list.List.Add(tex);
                    }
                }
            }
        }

        [Button] void Optimize() { }
    }
}
