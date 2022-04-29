using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static RekornTools.Avatar.EditorGUILayoutExtensions;

namespace RekornTools.Avatar.Editor
{
    partial class LightScenario
    {
        [CustomEditor(typeof(LightScenario))]
        sealed class LightScenarioDrawer : SerializedEditor<LightScenario>
        {
            protected override void Draw(LightScenario t)
            {
                LabelField("Light Scenario", EditorStyles.boldLabel);

                t.Skybox              = ObjectField("Skybox", t.Skybox, false);
                t.UseShadow           = Toggle("Use Shadow", t.UseShadow);
                t.ReflectionIntensity = Slider("Reflection Intensity", t.ReflectionIntensity, 0, 1);
                t.SunDirection        = Vector3Field("Sun Direction", t.SunDirection);

                HorizontalLine();

                BeginHorizontal();
                {
                    LabelField("Sun Light", GUILayout.Width(100));
                    t.SyncColor    = Toggle(t.SyncColor, GUILayout.Width(20));
                    t.SunColor     = ColorField(t.SunColor, GUILayout.Width(60));
                    t.SunIntensity = Slider(t.SunIntensity, 0f, 8f);
                }
                EndHorizontal();

                BeginHorizontal();
                {
                    LabelField("Ambient Light", GUILayout.Width(100));

                    if (t.SyncColor)
                    {
                        LabelField("Color Synced", EditorStyles.helpBox, GUILayout.Width(80));
                    }
                    else
                    {
                        t.UseSkyboxColor = Toggle(t.UseSkyboxColor, GUILayout.Width(20));

                        if (t.UseSkyboxColor) LabelField("Skybox", EditorStyles.helpBox, GUILayout.Width(60));
                        else t.SkyColor = ColorField(t.SkyColor, GUILayout.Width(60));
                    }

                    t.SkyboxIntensity = Slider(t.SkyboxIntensity, 0f, 8f);
                }
                EndHorizontal();
            }
        }
    }
}
