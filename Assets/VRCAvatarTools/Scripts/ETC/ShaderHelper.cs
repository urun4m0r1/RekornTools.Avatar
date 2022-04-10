#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace VRCAvatarTools
{
    [ExecuteInEditMode]
    public sealed class ShaderHelper : MonoBehaviour
    {
        [SerializeField]           private string           shaderPath              = "UnityChanToonShader/Toon_DoubleShadeWithFeather";
        [SerializeField]           private string           mainTextureProperty     = "_MainTex";
        [SerializeField]           private string           emissiveTextureProperty = "_Emissive_Tex";
        [SerializeField, ReadOnly] private List<GameObject> gameObjects;
        [SerializeField, ReadOnly] private List<Renderer>   renderers;
        [SerializeField, ReadOnly] private List<Material>   materials;
        [SerializeField, ReadOnly] private List<Material>   shaders;
        [SerializeField]           private List<Material>   ignoreShaders = new List<Material>();

        [Button]
        [UsedImplicitly]
        private void UpdateShaders()
        {
            InitFields();

            //gameObjects = GameObjectExtensions.GetAllGameObjectsInScene.ToList();
            if (gameObjects.Count < 1)
            {
                Debug.LogError("No GameObjects in scene!");
                return;
            }

            foreach (Renderer result in
                     from g in gameObjects
                     from r in g.GetComponents<Renderer>()
                     where !renderers.Contains(r)
                     select r) renderers.Add(result);
            if (renderers.Count < 1)
            {
                Debug.LogError("No Renderer in scene!");
                return;
            }

            foreach (Material result in
                     from r in renderers
                     from m in r.sharedMaterials
                     where !materials.Contains(m)
                     select m) materials.Add(result);
            if (materials.Count < 1)
            {
                Debug.LogError("No Material in scene!");
                return;
            }

            Shader shader = Shader.Find(shaderPath);
            if (shader == null)
            {
                Debug.LogError($"Failed to find shader with name: {shaderPath}");
                return;
            }

            foreach (Material result in
                     from r in materials
                     where r.shader == shader
                     select r) shaders.Add(result);
            if (shaders.Count < 1)
            {
                Debug.LogError($"No Material using {shaderPath} in scene!");
                return;
            }
        }

        private void InitFields()
        {
            gameObjects = null;
            renderers   = new List<Renderer>();
            materials   = new List<Material>();
            shaders     = new List<Material>();
        }

        [Button]
        [UsedImplicitly]
        private void SetEmissiveTextureFromMainTexture()
        {
            if (shaders.Count < 1)
            {
                Debug.LogError($"No Material using {shaderPath} in scene!");
                return;
            }

            var mainTextureID     = Shader.PropertyToID(mainTextureProperty);
            var emissiveTextureID = Shader.PropertyToID(emissiveTextureProperty);


            foreach (Material s in shaders)
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

                Texture mainTexture = s.GetTexture(mainTextureID);

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
