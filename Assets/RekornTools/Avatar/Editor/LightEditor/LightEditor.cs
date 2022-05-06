using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.EditorGUILayout;
using static RekornTools.Avatar.Editor.EditorGUILayoutExtensions;

namespace RekornTools.Avatar.Editor
{
    public sealed class LightEditor : BaseEditorWindow<LightEditor>
    {
        [SerializeField] bool          _ignoreLighting;
        [SerializeField] Light         _sun;
        [SerializeField] LightScenario _scenario;

        [MenuItem("Tools/VRC Avatar Tools/Light Editor")]
        static void OnWindowOpen() =>
            GetWindow<LightEditor>("Light Editor")?.Show();

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

        protected override void Draw()
        {
            LabelField("Light Preset", EditorStyles.boldLabel);

            _ignoreLighting = Toggle("Ignore Lighting", _ignoreLighting);
            SetCurrentSceneViewLighting(!_ignoreLighting);

            _scenario = ObjectField("Scenario", _scenario, false);
            DrawLightScenario();
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

        void DrawLightScenario()
        {
            if (_scenario == null) return;

            HorizontalLine();
            _scenario.DrawEditor(_ignoreLighting);
            _scenario.Apply(_sun);
        }
    }
}
