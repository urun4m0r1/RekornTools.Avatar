using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace RekornTools.Avatar
{
    public static class ShaderPropertyExtensions
    {
        [CanBeNull]
        public static IEnumerable<Shader> AllUserShadersInProject =>
            from material in AllMaterialsInProject
            let shader = material.shader
            where shader != null
            select shader;

        [CanBeNull]
        public static IEnumerable<Material> AllMaterialsInProject =>
            from guid in AssetDatabase.FindAssets("t:Material")
            let path = AssetDatabase.GUIDToAssetPath(guid)
            let material = AssetDatabase.LoadAssetAtPath<Material>(path)
            where material != null
            select material;

        [CanBeNull]
        public static IEnumerable<Shader> AllShadersInProject =>
            from guid in AssetDatabase.FindAssets("t:Shader")
            let path = AssetDatabase.GUIDToAssetPath(guid)
            let shader = AssetDatabase.LoadAssetAtPath<Shader>(path)
            where shader != null
            select shader;

        [CanBeNull]
        public static TextureProperties GetTexturePropertyList([CanBeNull] this Shader shader)
        {
            if (shader == null) return null;

            var count = shader.GetPropertyCount();
            if (count == 0) return null;

            var properties = new TextureProperties();
            for (var i = 0; i < count; i++)
            {
                if (shader.GetPropertyFlags(i) == ShaderPropertyFlags.HideInInspector) continue;
                if (shader.GetPropertyType(i)  != ShaderPropertyType.Texture) continue;
                properties.Add(ShaderProperty.Create(shader, i) as TextureProperty);
            }

            return properties.Count == 0 ? null : properties;
        }
    }
}
