using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static RekornTools.Avatar.Editor.EditorGUILayoutExtensions;
using static UnityEditor.EditorGUILayout;

namespace RekornTools.Avatar.Editor
{
    [CustomEditor(typeof(AssetManager))]
    public sealed class AssetManagerEditor : BaseEditor<AssetManager>
    {
        bool _renderersFoldout;
        bool _materialsFoldout;
        bool _shadersFoldout;
        bool _texturesFoldout;

        protected override void Draw(AssetManager t)
        {
            t.Parent = ObjectField("Parent", t.Parent, true);

            DrawLists(ref _renderersFoldout, nameof(AssetManager.Renderers), t.Renderers);
            DrawLists(ref _materialsFoldout, nameof(AssetManager.Materials), t.Materials);
            DrawLists(ref _shadersFoldout,   nameof(AssetManager.Shaders),   t.Shaders);
            DrawLists(ref _texturesFoldout,  nameof(AssetManager.Textures),  t.Textures);
        }

        void DrawLists<T>(ref bool foldout, [NotNull] string property, [NotNull] ObjectList<T> list) where T : Object
        {
            BeginHorizontal();
            {
                foldout = Foldout(foldout, typeof(T).Name);
                if (GUILayout.Button("Select All")) list.SelectComponents();
            }
            EndHorizontal();
            if (foldout) PropertyField(serializedObject.ResolveProperty(property), true);
        }
    }
}
