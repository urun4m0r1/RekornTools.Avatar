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

            if (t.Meshes.Count > 0)
            {
                DrawBoneFinder(t);
                HorizontalLine();
                DrawWeightedBoneFinder(t);
            }
        }

        static void DrawMeshFinder([NotNull] BoneFinder t)
        {
            BeginHorizontal();
            {
                LabelField("Mesh Finder", EditorStyles.boldLabel);
                if (GUILayout.Button("Find")) t.FindMeshesFromTargetWithKeyword();
            }
            EndHorizontal();

            t.MeshParent  = ObjectField("Parent", t.MeshParent, true);
            t.MeshKeyword = TextField("Keyword", t.MeshKeyword);
        }

        static void DrawBoneFinder([NotNull] BoneFinder t)
        {
            BeginHorizontal();
            {
                LabelField("Bone Finder", EditorStyles.boldLabel);
                if (GUILayout.Button("Find")) t.FindBonesFromTargetWithKeyword();
            }
            EndHorizontal();

            t.BoneParent  = ObjectField("Parent", t.BoneParent, true);
            t.BoneKeyword = TextField("Keyword", t.BoneKeyword);
        }

        static void DrawWeightedBoneFinder([NotNull] BoneFinder t)
        {
            BeginHorizontal();
            {
                LabelField("Bone Finder (From Weights)", EditorStyles.boldLabel);
                if (GUILayout.Button("Find")) t.FindBonesFromWeights();
                if (GUILayout.Button("Find Recursive")) t.FindBonesFromWeightsRecursive();
            }
            EndHorizontal();
        }
    }
}
