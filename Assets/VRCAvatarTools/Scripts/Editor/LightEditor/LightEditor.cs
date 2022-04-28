﻿using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.EditorGUILayout;
using static VRCAvatarTools.EditorGUILayoutExtensions;

namespace VRCAvatarTools.Editor
{
    internal sealed class LightEditor : SerializedEditorWindow<LightEditor>
    {
        [SerializeField] private bool          _ignoreLighting;
        [SerializeField] private Light         _sun;
        [SerializeField] private LightScenario _scenario;

        [MenuItem("Tools/VRC Avatar Tools/Light Editor")] private static void OnWindowOpen() =>
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

        private void OnSceneChanged(Scene prev, Scene current) => FindSun();

        private void FindSun()
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

        private static void SetCurrentSceneViewLighting(bool isEnable)
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

        private void DrawLightScenario()
        {
            if (!_scenario) return;

            HorizontalLine();
            _scenario.DrawEditor(_ignoreLighting);
            _scenario.Apply(_sun);
        }
    }
}
