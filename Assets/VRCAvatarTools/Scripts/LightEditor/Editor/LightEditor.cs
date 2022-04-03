using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.EditorGUILayout;
using static VRCAvatarTools.EditorGUILayoutExtensions;

namespace VRCAvatarTools
{
    public class LightEditor : EditorWindow
    {
        static bool          _ignoreLighting;
        static Light         _sun;
        static LightScenario _scenario;

        [MenuItem("Tools/VRC Avatar Tools/Light Editor")]
        static void OnWindowOpen()
        {
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
            GetWindow<LightEditor>("Light Editor")?.Show();
        }

        static void OnSceneChanged(Scene prev, Scene current) => FindSun();

        void OnEnable() => FindSun();

        static void FindSun()
        {
            const string sunName = "Directional Light";
            _sun = GameObject.Find(sunName)?.GetComponent<Light>();
            if (_sun == null) _sun = new GameObject(sunName).AddComponent<Light>();
        }

        public void OnGUI()
        {
            LabelField("Light Preset", EditorStyles.boldLabel);
            _ignoreLighting = Toggle("Ignore Lighting", _ignoreLighting);
            SetCurrentSceneViewLighting(!_ignoreLighting);

            EditorGUI.BeginDisabledGroup(_ignoreLighting);
            {
                _scenario = ObjectField("Scenario", _scenario, false);
                if (!_scenario) return;

                HorizontalLine();

                _scenario.DrawEditor();
            }
            EditorGUI.EndDisabledGroup();

            _scenario.Apply(_sun);
        }

        static void SetCurrentSceneViewLighting(bool isEnable)
        {
            var sceneViews = SceneView.sceneViews;
            if (sceneViews == null) return;

            foreach (SceneView view in sceneViews)
            {
                if (view == null) continue;

                view.sceneLighting = isEnable;
                view.Repaint();
            }
        }
    }
}
