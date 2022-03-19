using System;
using UnityEngine;

namespace VRCAvatarTools
{
    [Serializable]
    public class CustomLight
    {
        public static bool UseSkybox;

        [SerializeField] public Vector3 sunDirection;
        [SerializeField] public float   sunIntensity;
        [SerializeField] public float   skyboxIntensity;
        [SerializeField] public Color   sunColor;
        [SerializeField] public Color   skyColor;

        public void SetLight(Light light)
        {
            if (light)
            {
                if (sunDirection == Vector3.zero) sunDirection = -Vector3.up;

                light.transform.rotation = Quaternion.LookRotation(sunDirection);
                light.color              = sunColor;
                light.intensity          = sunIntensity;
            }

            RenderSettings.ambientIntensity = skyboxIntensity;
            RenderSettings.ambientLight     = skyColor * skyboxIntensity;
        }

        public CustomLight() { }

        public CustomLight(CustomLight preset)
        {
            if (preset == null) preset = Day;

            sunDirection    = preset.sunDirection;
            sunIntensity    = preset.sunIntensity;
            sunColor        = preset.sunColor;
            skyboxIntensity = preset.skyboxIntensity;
            skyColor        = preset.skyColor;
        }

        public CustomLight(Vector3 sunDirection, float sunIntensity, float skyboxIntensity, Color sunColor,
            Color                  skyColor)
        {
            this.sunDirection    = sunDirection;
            this.sunIntensity    = sunIntensity;
            this.skyboxIntensity = skyboxIntensity;
            this.sunColor        = sunColor;
            this.skyColor        = skyColor;
        }

        static readonly Vector3 SunDirection    = new Vector3(-0.5f, -1f, 1f);
        static readonly Vector3 SunsetDirection = new Vector3(-0.5f, 0f,  1f);

        public static readonly CustomLight Day
            = new CustomLight(SunDirection,
                5.0f, 1.0f, Color.white, Color.yellow);

        public static readonly CustomLight Night
            = new CustomLight(-SunDirection,
                0.1f, 0.1f, Color.white, Color.blue);

        public static readonly CustomLight Sunset
            = new CustomLight(SunsetDirection,
                0.8f, 0.5f, Color.yellow, Color.red);

        public static readonly CustomLight Sunrise
            = new CustomLight(-SunsetDirection,
                0.8f, 0.5f, Color.yellow, Color.cyan);

        public static readonly CustomLight PureBlack
            = new CustomLight(-SunDirection,
                0.0f, 0.0f, Color.black, Color.black);

        public static readonly CustomLight PureWhite
            = new CustomLight(SunDirection,
                1.0f, 1.0f, Color.white, Color.white);

        public static readonly CustomLight FullBrightness
            = new CustomLight(SunDirection,
                8.0f, 8.0f, Color.white, Color.white);

        public static CustomLight GetLightSettings(LightPreset type)
        {
            switch (type)
            {
                case LightPreset.Custom:         return null;
                case LightPreset.Day:            return Day;
                case LightPreset.Night:          return Night;
                case LightPreset.Sunset:         return Sunset;
                case LightPreset.Sunrise:        return Sunrise;
                case LightPreset.PureBlack:      return PureBlack;
                case LightPreset.PureWhite:      return PureWhite;
                case LightPreset.FullBrightness: return FullBrightness;
                default:                         throw new ArgumentOutOfRangeException();
            }
        }
    }
}
