using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    public static class ShaderPropertyExtensions
    {
        public static List<Shader> GetUsedShadersInProject()
        {
            var shaders = new List<Shader>();

            foreach (Shader shader in AllMaterialsInProject
                                     .Where(x => x)
                                     .Select(x => x.shader)
                                     .Where(shader => !shaders.Contains(shader)))
                shaders.Add(shader);

            return shaders;
        }

        public static IEnumerable<Material> AllMaterialsInProject =>
            from guid in AssetDatabase.FindAssets("t:Material")
            let path = AssetDatabase.GUIDToAssetPath(guid)
            let material = AssetDatabase.LoadAssetAtPath<Material>(path)
            where material
            select material;

        public static IEnumerable<Shader> AllShadersInProject =>
            from guid in AssetDatabase.FindAssets("t:Shader")
            let path = AssetDatabase.GUIDToAssetPath(guid)
            let shader = AssetDatabase.LoadAssetAtPath<Shader>(path)
            where shader
            select shader;

        public static ShaderPropertyList GetTexturePropertyList(this Shader shader)
        {
            var properties = new ShaderPropertyList(shader);

            if (shader.TryGetShaderPropertyCount(out int count))
                for (var i = 0; i < count; i++)
                    properties.AddTextureProperty(shader, i);
            return properties;
        }

        public static void AddTextureProperty(this ShaderPropertyList list, Shader shader, int i)
        {
            if (shader.GetPropertyFlags(i) == ShaderPropertyFlags.HideInInspector)
                return;

            if (shader.GetPropertyType(i) != ShaderPropertyType.Texture)
                return;

            list.Add(GetShaderPropertyAt(shader, i));
        }

        public static ShaderProperty GetShaderPropertyAt(this Shader shader, int i) =>
            new ShaderProperty(shader, i, shader.GetPropertyName(i), shader.GetPropertyType(i));

        public static bool TryGetShaderPropertyCount(this Shader shader, out int propertyCount)
        {
            if (!shader)
            {
                propertyCount = 0;
                return false;
            }

            propertyCount = shader.GetPropertyCount();
            return true;
        }
    }
}
