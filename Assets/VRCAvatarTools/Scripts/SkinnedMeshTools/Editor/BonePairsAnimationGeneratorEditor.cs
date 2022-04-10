using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    [CustomEditor(typeof(BonePairsAnimationGenerator))]
    public class BonePairsAnimationGeneratorEditor : UnityEditor.Editor
    {
        private BonePairsAnimationGenerator _target;

        private const string ClassName = nameof(BonePairsAnimationGenerator);

        private void OnEnable() => _target = (BonePairsAnimationGenerator)target;

        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 5;

            EditorGUILayout.LabelField("Create Animation", EditorStyles.boldLabel);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Pairs", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("Toggle")) _target.CreatePairsToggleAnimation();
                    if (GUILayout.Button("ON")) _target.CreatePairsEnableAnimation();
                    if (GUILayout.Button("OFF")) _target.CreatePairsDisableAnimation();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Meshes", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("Toggle")) _target.CreateMeshesToggleAnimation();
                    if (GUILayout.Button("ON")) _target.CreateMeshesEnableAnimation();
                    if (GUILayout.Button("OFF")) _target.CreateMeshesDisableAnimation();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Bones", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("Toggle")) _target.CreateBonesToggleAnimation();
                    if (GUILayout.Button("ON")) _target.CreateBonesEnableAnimation();
                    if (GUILayout.Button("OFF")) _target.CreateBonesDisableAnimation();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
