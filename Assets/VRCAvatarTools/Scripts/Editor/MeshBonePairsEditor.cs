using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomEditor(typeof(MeshBonePairs))]
    public class MeshBonePairsEditor : Editor
    {
        MeshBonePairs _target;
        bool          _meshesFoldout = true;
        bool          _bonesFoldout  = false;

        void OnEnable() => _target = (MeshBonePairs)target;

        static SerializedProperty FindPropertyByAutoPropertyName(SerializedObject obj, string propName) =>
            obj.FindProperty($"<{propName}>k__BackingField");

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
                        EditorGUILayout.PropertyField(FindPropertyByAutoPropertyName(serializedObject, "Meshes"), true);
                    }
                    EditorGUI.indentLevel--;
                }

                _bonesFoldout = EditorGUILayout.Foldout(_bonesFoldout, "Bones");
                if (_bonesFoldout)
                {
                    EditorGUI.indentLevel++;
                    {
                        EditorGUILayout.PropertyField(FindPropertyByAutoPropertyName(serializedObject, "Bones"), true);
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
