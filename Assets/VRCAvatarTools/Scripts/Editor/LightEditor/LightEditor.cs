using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.EditorGUILayout;
using static VRCAvatarTools.EditorGUILayoutExtensions;

namespace VRCAvatarTools.Editor
{
    sealed class LightEditor : SerializedEditorWindow<LightEditor>
    {
        [SerializeField] bool          _ignoreLighting;
        [SerializeField] Light         _sun;
        [SerializeField] LightScenario _scenario;

        protected override void Enable()
        {
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
            FindSun();
        }

        protected override void Disable()
        {
            EditorSceneManager.activeSceneChangedInEditMode -= OnSceneChanged;
        }

        void OnSceneChanged(Scene prev, Scene current) => FindSun();

        void FindSun()
        {
            const string sunName = "Directional Light";
            _sun = GameObject.Find(sunName)?.GetComponent<Light>();
            if (_sun == null) _sun = new GameObject(sunName).AddComponent<Light>();
        }

        [MenuItem("Tools/VRC Avatar Tools/Light Editor")]
        static void OnWindowOpen() => GetWindow<LightEditor>("Light Editor")?.Show();

        protected override void Draw()
        {
            LabelField("Light Preset", EditorStyles.boldLabel);

            _ignoreLighting = Toggle("Ignore Lighting", _ignoreLighting);
            SetCurrentSceneViewLighting(!_ignoreLighting);

            _scenario = ObjectField("Scenario", _scenario, false);
            DrawLightScenario();
        }

        void DrawLightScenario()
        {
            EditorGUI.BeginDisabledGroup(_ignoreLighting);
            {
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
