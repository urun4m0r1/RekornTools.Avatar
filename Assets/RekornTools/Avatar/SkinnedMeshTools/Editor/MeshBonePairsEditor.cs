using JetBrains.Annotations;
using UnityEditor;
using static UnityEditor.EditorGUILayout;
using static UnityEngine.GUILayout;

namespace RekornTools.Avatar.Editor
{
    [CustomEditor(typeof(MeshBonePairs))]
    public sealed class MeshBonePairsEditor : BaseEditor<MeshBonePairs>
    {
        bool _meshesFoldout = true;
        bool _bonesFoldout  = false;

        protected override void Draw(MeshBonePairs t)
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 5;

            DrawLists();
            DrawButton(t);
        }

        void DrawLists()
        {
            LabelField("Lists", EditorStyles.boldLabel);
            {
                _meshesFoldout = Foldout(_meshesFoldout, "Meshes");
                if (_meshesFoldout) PropertyField(serializedObject.ResolveProperty(nameof(MeshBonePairs.Meshes)), true);

                _bonesFoldout = Foldout(_bonesFoldout, "Bones");
                if (_bonesFoldout) PropertyField(serializedObject.ResolveProperty(nameof(MeshBonePairs.Bones)), true);
            }
        }

        static void DrawButton([NotNull] MeshBonePairs t)
        {
            LabelField("Actions", EditorStyles.boldLabel);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    LabelField("Select Items", EditorStyles.miniBoldLabel);
                    if (Button("All")) t.SelectAll();
                    if (Button("Meshes")) t.SelectMeshes();
                    if (Button("Bones")) t.SelectBones();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    LabelField("Clear List", EditorStyles.miniBoldLabel);
                    if (Button("All")) t.ClearAll();
                    if (Button("Meshes")) t.ClearMeshes();
                    if (Button("Bones")) t.ClearBones();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    LabelField("Destroy Items", EditorStyles.miniBoldLabel);
                    if (Button("All")) t.DestroyAll();
                    if (Button("Meshes")) t.DestroyMeshes();
                    if (Button("Bones")) t.DestroyBones();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
