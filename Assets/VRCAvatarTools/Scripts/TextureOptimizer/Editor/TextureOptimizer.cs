using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    public class TextureOptimizer : SerializedEditorWindow<TextureOptimizer>
    {
        [SerializeField] private Vector2                 _scrollPosition = Vector2.zero;
        [SerializeField] private TextureImporterSettings _settings;
        [SerializeField] private ShaderPropertyObject    _obj;

        [MenuItem("Tools/VRC Avatar Tools/Texture Optimizer")] private static void OnWindowOpen() => GetWindow<TextureOptimizer>("Texture Optimizer")?.Show();

        protected override void Draw()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                _settings = _settings.GetInstance();
                _settings.DrawEditor();

                _obj = _obj.GetInstance();
                DrawShaderPropertyMap(_obj);
            }
            GUILayout.EndScrollView();
        }

        private static void DrawShaderPropertyMap([NotNull] ShaderPropertyObject shaderPropertyObject)
        {
            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Update Shaders"))
                {
                    shaderPropertyObject.UpdateShaders();
                }

                if (GUILayout.Button("Reset Items"))
                {
                    shaderPropertyObject.ResetItems();
                }
            }
            GUILayout.EndHorizontal();

            shaderPropertyObject.DrawEditor();
        }
    }
}
