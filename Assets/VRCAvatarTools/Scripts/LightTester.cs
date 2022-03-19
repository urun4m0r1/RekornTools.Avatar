using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace VRCAvatarTools
{
    public enum LightPreset
    {
        Custom,
        Day,
        Night,
        Sunset,
        Sunrise,
        PureBlack,
        PureWhite,
        FullBrightness,
    }

    [ExecuteInEditMode]
    public class LightTester : MonoBehaviour
    {
        [SerializeField]                LightPreset preset = LightPreset.Custom;
        [SerializeField]                Material    skybox;
        [SerializeField]                Light       sun;
        [SerializeField]                bool        useSkybox           = true;
        [SerializeField]                bool        useShadow           = true;
        [SerializeField, Range(0f, 1f)] float       reflectionIntensity = 0.5f;

        [SerializeField] CustomLight customSettings = new CustomLight(CustomLight.PureWhite);

        void Awake()
        {
            CustomLight.UseSkybox = useSkybox;
            if (sun    == null) sun    = FindObjectOfType<Light>();
            if (skybox == null) skybox = RenderSettings.skybox;
        }

        void OnValidate()
        {
            CustomLight.UseSkybox = useSkybox;

            SceneView.lastActiveSceneView.sceneLighting = true;

            RenderSettings.skybox              = skybox;
            RenderSettings.sun                 = sun;
            RenderSettings.reflectionIntensity = reflectionIntensity;
            RenderSettings.ambientMode         = useSkybox ? AmbientMode.Skybox : AmbientMode.Flat;

            if (sun)
            {
                sun.gameObject.SetActive(true);
                sun.shadowStrength = 1f;
                sun.shadows        = useShadow ? LightShadows.Soft : LightShadows.None;
            }

            CustomLight settings = CustomLight.GetLightSettings(preset) ?? customSettings;

            settings.SetLight(sun);
        }
    }
}
