using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomEditor(typeof(BoneFinder))]
    public class BoneFinderEditor : UnityEditor.Editor
    {
        BoneFinder _target;

        const string ClassName = nameof(BoneFinder);

        void OnEnable() => _target = (BoneFinder)target;

        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 5;

            EditorGUILayout.LabelField("Meshes Finder", EditorStyles.boldLabel);
            {
                EditorGUILayout.LabelField("From Hierarchy", EditorStyles.miniBoldLabel);
                {
                    Undo.RecordObject(_target, ClassName);
                    {
                        _target.MeshParent =
                            EditorGUILayout.ObjectField(
                                "Parent",
                                _target.MeshParent,
                                typeof(Transform),
                                true) as Transform;
                        _target.MeshKeyword = EditorGUILayout.TextField("Keyword", _target.MeshKeyword);
                    }
                    if (GUILayout.Button("Find")) _target.FindMeshesFromTargetWithKeyword();
                }
            }

            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Bones Finder", EditorStyles.boldLabel);
            {
                EditorGUILayout.LabelField("From Painted Weights", EditorStyles.miniBoldLabel);
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Find"))
                            _target.FindBonesFromMeshesWeights();
                        if (GUILayout.Button("Find Including Children"))
                            _target.FindBonesFromMeshesWeightsIncludingChildren();
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.Space(10);

                EditorGUILayout.LabelField("From Hierarchy", EditorStyles.miniBoldLabel);
                {
                    Undo.RecordObject(_target, ClassName);
                    {
                        _target.BoneParent =
                            EditorGUILayout.ObjectField(
                                "Parent",
                                _target.BoneParent,
                                typeof(Transform),
                                true) as Transform;
                        _target.BoneKeyword = EditorGUILayout.TextField("Keyword", _target.BoneKeyword);
                    }
                    if (GUILayout.Button("Find")) _target.FindBonesFromTargetWithKeyword();
                }
            }
        }
    }
}
