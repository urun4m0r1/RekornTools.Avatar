using UnityEditor;
using UnityEngine;
using static VRCAvatarTools.EditorGUILayoutExtensions;

namespace VRCAvatarTools.Editor
{
    public class TextureOptimizer : SerializedEditorWindow<TextureOptimizer>
    {
        [SerializeField] private TexturePresetSettings _presetSettings;
        [SerializeField] private ShaderPropertiesTable _shaderPropertiesTable;

        [MenuItem("Tools/VRC Avatar Tools/Texture Optimizer")]
        private static void OnWindowOpen() => GetWindow<TextureOptimizer>("Texture Optimizer")?.Show();

        protected override void Draw()
        {
            _presetSettings = ObjectField("", _presetSettings, false);
            if (_presetSettings) _presetSettings.DrawEditor();

            _shaderPropertiesTable = ObjectField("", _shaderPropertiesTable, false);
            if (_shaderPropertiesTable)
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Update Shaders")) _shaderPropertiesTable.UpdateShaders();
                    if (GUILayout.Button("Reset Items")) _shaderPropertiesTable.ResetItems();
                }
                GUILayout.EndHorizontal();
            
                _shaderPropertiesTable.DrawEditor();
            }
        }
    }
}
