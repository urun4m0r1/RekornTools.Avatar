using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;

namespace RekornTools.Avatar.Editor
{
    [CustomEditor(typeof(MeshBonePairs))]
    public sealed class MeshBonePairsEditor : BaseEditor<MeshBonePairs>
    {
        bool _meshesFoldout = true;
        bool _bonesFoldout = false;

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
                BeginHorizontal();
                {
                    LabelField("Select Items", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("All")) t.SelectAll();
                    if (GUILayout.Button("Meshes")) t.SelectMeshes();
                    if (GUILayout.Button("Bones")) t.SelectBones();
                }
                EndHorizontal();

                BeginHorizontal();
                {
                    LabelField("Clear List", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("All")) t.ClearAll();
                    if (GUILayout.Button("Meshes")) t.ClearMeshes();
                    if (GUILayout.Button("Bones")) t.ClearBones();
                }
                EndHorizontal();

                BeginHorizontal();
                {
                    LabelField("Destroy Items", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("All")) t.DestroyAll();
                    if (GUILayout.Button("Meshes")) t.DestroyMeshes();
                    if (GUILayout.Button("Bones")) t.DestroyBones();
                }
                EndHorizontal();
            }
        }
    }
}
