using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    [CreateAssetMenu(menuName = "VRC Avatar Tools/Light Scenario")]
    public partial class LightScenario : ScriptableObject
    {
        [field: SerializeField] public Material Skybox              { get; private set; }
        [field: SerializeField] public bool     UseShadow           { get; private set; }
        [field: SerializeField] public float    ReflectionIntensity { get; private set; }

        [field: SerializeField] public Vector3  SunDirection        { get; private set; }

        [field: SerializeField] public bool     SyncColor           { get; private set; }
        [field: SerializeField] public Color    SunColor            { get; private set; }
        [field: SerializeField] public float    SunIntensity        { get; private set; }

        [field: SerializeField] public bool  UseSkyboxColor  { get; private set; }
        [field: SerializeField] public Color SkyColor        { get; private set; }
        [field: SerializeField] public float SkyboxIntensity { get; private set; }

        public AmbientMode AmbientMode => UseSkyboxColor && !SyncColor ? AmbientMode.Skybox : AmbientMode.Flat;

        public void OnValidate()
        {
            var direction = SunDirection;
            direction.x  = Mathf.Clamp(direction.x, -1f, 1f);
            direction.y  = Mathf.Clamp(direction.y, -1f, 1f);
            direction.z  = Mathf.Clamp(direction.z, -1f, 1f);
            if (direction == Vector3.zero) direction = -Vector3.up;
            SunDirection = direction;
        }

        public void Apply([CanBeNull] Light light)
        {
            RenderSettings.skybox              = Skybox;
            RenderSettings.reflectionIntensity = ReflectionIntensity;
            RenderSettings.ambientMode         = AmbientMode;
            RenderSettings.ambientIntensity    = SkyboxIntensity;
            RenderSettings.ambientLight        = (SyncColor ? SunColor : SkyColor) * SkyboxIntensity;
            RenderSettings.sun                 = light;

            if (light)
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
