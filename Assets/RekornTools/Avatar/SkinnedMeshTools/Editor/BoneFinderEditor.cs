using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using static RekornTools.Avatar.Editor.EditorGUILayoutExtensions;
using static UnityEditor.EditorGUILayout;

namespace RekornTools.Avatar.Editor
{
    [CustomEditor(typeof(BoneFinder))]
    public sealed class BoneFinderEditor : BaseEditor<BoneFinder>
    {
        protected override void Draw(BoneFinder t)
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 5;

            DrawMeshFinder(t);
            HorizontalLine();

            if (t.Meshes.Count > 0) DrawBoneFinder(t);
        }

        static void DrawMeshFinder([NotNull] BoneFinder t)
        {
            BeginHorizontal();
            {
                LabelField("Mesh Finder", EditorStyles.boldLabel);
                if (GUILayout.Button("Find")) t.FindMeshesFromTargetWithKeyword();
            }
            EndHorizontal();

            t.UndoableAction(() =>
            {
                t.MeshParent  = ObjectField("Parent", t.MeshParent, true);
                t.MeshKeyword = TextField("Keyword", t.MeshKeyword);
            });
        }

        static void DrawBoneFinder([NotNull] BoneFinder t)
        {
            BeginHorizontal();
            {
                LabelField("Bone Finder", EditorStyles.boldLabel);
                if (GUILayout.Button("Find")) t.FindBonesFromTargetWithKeyword();
            }
            EndHorizontal();

            t.UndoableAction(() =>
            {
                t.BoneParent  = ObjectField("Parent", t.BoneParent, true);
                t.BoneKeyword = TextField("Keyword", t.BoneKeyword);
            });

            HorizontalLine();

            BeginHorizontal();
            {
                LabelField("Bone Finder (From Weights)", EditorStyles.boldLabel);
                if (GUILayout.Button("Find")) t.FindBonesFromMeshesWeights();
                if (GUILayout.Button("Find Recursive")) t.FindBonesFromMeshesWeightsIncludingChildren();
            }
            EndHorizontal();
        }
    }
}
