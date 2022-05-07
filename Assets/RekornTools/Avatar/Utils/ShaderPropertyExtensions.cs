using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

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
    }
}
