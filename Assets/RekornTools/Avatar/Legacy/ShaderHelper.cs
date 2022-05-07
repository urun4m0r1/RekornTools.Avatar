#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class ShaderHelper : MonoBehaviour
    {
        [SerializeField]           string           shaderPath              = "UnityChanToonShader/Toon_DoubleShadeWithFeather";
        [SerializeField]           string           mainTextureProperty     = "_MainTex";
        [SerializeField]           string           emissiveTextureProperty = "_Emissive_Tex";
        [SerializeField] List<GameObject> gameObjects;
        [SerializeField] List<Renderer>   renderers;
        [SerializeField] List<Material>   materials;
        [SerializeField] List<Material>   shaders;
        [SerializeField]           List<Material>   ignoreShaders = new List<Material>();

        [Button]
        [UsedImplicitly]
        void UpdateShaders()
        {
            InitFields();

            gameObjects = GameObjectExtensions.GetAllGameObjectsInScene.ToList();
            if (gameObjects.Count < 1)
            {
                Debug.LogError("No GameObjects in scene!");
                return;
            }

            foreach (var result in
                     from g in gameObjects
                     from r in g.GetComponents<Renderer>()
                     where !renderers.Contains(r)
                     select r) renderers.Add(result);
            if (renderers.Count < 1)
            {
                Debug.LogError("No Renderer in scene!");
                return;
            }

            foreach (var result in
                     from r in renderers
                     from m in r.sharedMaterials
                     where !materials.Contains(m)
                     select m) materials.Add(result);
            if (materials.Count < 1)
            {
                Debug.LogError("No Material in scene!");
                return;
            }

            var shader = Shader.Find(shaderPath);
            if (shader == null)
            {
                Debug.LogError($"Failed to find shader with name: {shaderPath}");
                return;
            }

            foreach (var result in
                     from r in materials
                     where r.shader == shader
                     select r) shaders.Add(result);
            if (shaders.Count < 1)
            {
                Debug.LogError($"No Material using {shaderPath} in scene!");
                return;
            }
        }

        void InitFields()
        {
            gameObjects = null;
            renderers   = new List<Renderer>();
            materials   = new List<Material>();
            shaders     = new List<Material>();
        }

        [Button]
        [UsedImplicitly]
        void SetEmissiveTextureFromMainTexture()
        {
            if (shaders.Count < 1)
            {
                Debug.LogError($"No Material using {shaderPath} in scene!");
                return;
            }

            var mainTextureID     = Shader.PropertyToID(mainTextureProperty);
            var emissiveTextureID = Shader.PropertyToID(emissiveTextureProperty);


            foreach (var s in shaders)
            {
                if (ignoreShaders.Contains(s)) continue;

                if (!s.HasProperty(mainTextureID))
                {
                    Debug.LogError($"No {mainTextureProperty} property find in {s.name}.");
                    continue;
                }

                if (!s.HasProperty(emissiveTextureID))
                {
                    Debug.LogError($"No {emissiveTextureProperty} property find in {s.name}.");
                    continue;
                }

                var mainTexture = s.GetTexture(mainTextureID);

                if (mainTexture == null)
                {
                    Debug.LogError($"Failed to get {mainTextureProperty} in {s.name}.");
                    continue;
                }

                s.SetTexture(emissiveTextureID, mainTexture);

                if (s.GetTexture(emissiveTextureID) == null)
                {
                    Debug.LogError($"Failed to set {emissiveTextureProperty} in {s.name}.");
                    continue;
                }
            }

            Debug.Log($"Successfully set all shaders main texture to emissive texture!");
        }
    }
}
#endif
