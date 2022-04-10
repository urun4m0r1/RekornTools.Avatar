using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    public class TextureOptimizer : SerializedEditorWindow<TextureOptimizer>
    {
        [SerializeField] [CanBeNull] private TexturePresetSettings _presetSettings;
        [SerializeField] [CanBeNull] private ShaderPropertiesTable _shaderPropertiesTable;

        [MenuItem("Tools/VRC Avatar Tools/Texture Optimizer")]
        private static void OnWindowOpen() => GetWindow<TextureOptimizer>("Texture Optimizer")?.Show();

        protected override void Draw()
        {
            _presetSettings = _presetSettings.GetInstance();
            _presetSettings.DrawEditor();

            _shaderPropertiesTable = _shaderPropertiesTable.GetInstance();
            DrawShaderPropertyMap(_shaderPropertiesTable);
        }

        private static void DrawShaderPropertyMap([NotNull] ShaderPropertiesTable shaderPropertiesTable)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Update Shaders")) shaderPropertiesTable.UpdateShaders();
                if (GUILayout.Button("Reset Items")) shaderPropertiesTable.ResetItems();
            }
            GUILayout.EndHorizontal();

            shaderPropertiesTable.DrawEditor();
        }
    }
}
