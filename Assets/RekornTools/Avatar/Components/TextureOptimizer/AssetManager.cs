#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RekornTools.Avatar
{
    [ExecuteInEditMode]
    public sealed class AssetManager : MonoBehaviour
    {
        [SerializeField] List<GameObject> gameObjects;
        [SerializeField] List<Renderer>   renderers;
        [SerializeField] List<Material>   materials;
        [SerializeField] List<Shader>     shaders;

        [Button]
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

            foreach (var result in
                     from r in materials
                     select r.shader) shaders.Add(result);
        }

        void InitFields()
        {
            gameObjects = null;
            renderers   = new List<Renderer>();
            materials   = new List<Material>();
            shaders     = new List<Shader>();
        }
    }
}
#endif
