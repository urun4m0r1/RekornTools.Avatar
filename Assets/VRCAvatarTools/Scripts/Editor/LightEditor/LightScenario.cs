using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools.Editor
{
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Light Scenario")]
    internal partial class LightScenario : ScriptableObject, IValidate
    {
        [SerializeField] private Material Skybox;
        [SerializeField] private bool     UseShadow;
        [SerializeField] private float    ReflectionIntensity;

        [SerializeField] private Vector3 SunDirection;

        [SerializeField] private bool  SyncColor;
        [SerializeField] private Color SunColor;
        [SerializeField] private float SunIntensity;

        [SerializeField] private bool  UseSkyboxColor;
        [SerializeField] private Color SkyColor;
        [SerializeField] private float SkyboxIntensity;

        private AmbientMode AmbientMode => UseSkyboxColor && !SyncColor ? AmbientMode.Skybox : AmbientMode.Flat;

        public void OnValidate()
        {
            var direction = SunDirection;
            direction.x = Mathf.Clamp(direction.x, -1f, 1f);
            direction.y = Mathf.Clamp(direction.y, -1f, 1f);
            direction.z = Mathf.Clamp(direction.z, -1f, 1f);
            if (direction == Vector3.zero) direction = -Vector3.up;
            SunDirection = direction;
        }

        internal void Apply([CanBeNull] Light light)
        {
            RenderSettings.skybox              = Skybox;
            RenderSettings.reflectionIntensity = ReflectionIntensity;
            RenderSettings.ambientMode         = AmbientMode;
            RenderSettings.ambientIntensity    = SkyboxIntensity;
            RenderSettings.ambientLight        = (SyncColor ? SunColor : SkyColor) * SkyboxIntensity;
            RenderSettings.sun                 = light;

            if (light != null)
            {
                light.gameObject.SetActive(true);
                light.shadowStrength     = 1f;
                light.shadows            = UseShadow ? LightShadows.Soft : LightShadows.None;
                light.transform.rotation = Quaternion.LookRotation(SunDirection);
                light.color              = SunColor;
                light.intensity          = SunIntensity;
            }
        }
    }
}
