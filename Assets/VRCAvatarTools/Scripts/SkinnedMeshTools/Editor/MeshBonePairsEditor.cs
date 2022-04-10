using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    [CustomEditor(typeof(MeshBonePairs))]
    public class MeshBonePairsEditor : UnityEditor.Editor
    {
        private MeshBonePairs _target;
        private bool          _meshesFoldout = true;
        private bool          _bonesFoldout  = false;

        private void OnEnable() => _target = (MeshBonePairs)target;


        public override void OnInspectorGUI()
        {
            EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth / 5;

            EditorGUILayout.LabelField("Lists", EditorStyles.boldLabel);
            {
                _meshesFoldout = EditorGUILayout.Foldout(_meshesFoldout, "Meshes");
                if (_meshesFoldout)
                {
                    EditorGUI.indentLevel++;
                    {
                        EditorGUI.BeginChangeCheck();
                        {
                            EditorGUILayout.PropertyField(
                                SerializationExtensions.ResolveProperty(serializedObject, "Meshes"),
                                true);
                        }
                        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
                    }
                    EditorGUI.indentLevel--;
                }

                _bonesFoldout = EditorGUILayout.Foldout(_bonesFoldout, "Bones");
                if (_bonesFoldout)
                {
                    EditorGUI.indentLevel++;
                    {
                        EditorGUI.BeginChangeCheck();
                        {
                            EditorGUILayout.PropertyField(
                                SerializationExtensions.ResolveProperty(serializedObject, "Bones"),
                                true);
                        }
                        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
                    }
                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Select Items", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("All")) _target.SelectAll();
                    if (GUILayout.Button("Meshes")) _target.SelectMeshes();
                    if (GUILayout.Button("Bones")) _target.SelectBones();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Clear List", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("All")) _target.ClearAll();
                    if (GUILayout.Button("Meshes")) _target.ClearMeshes();
                    if (GUILayout.Button("Bones")) _target.ClearBones();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Destroy Items", EditorStyles.miniBoldLabel);
                    if (GUILayout.Button("All")) _target.DestroyAll();
                    if (GUILayout.Button("Meshes")) _target.DestroyMeshes();
                    if (GUILayout.Button("Bones")) _target.DestroyBones();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
