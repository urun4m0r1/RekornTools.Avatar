#if UNITY_EDITOR
using System;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public sealed partial class AutoDresser
    {
        // [CustomEditor(typeof(AutoDresser))]
        public sealed class Editor : UnityEditor.Editor
        {
            private static AutoDresser _target;

            private void OnEnable()
            {
                _target = target as AutoDresser;
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();

                if (_target == null) return;

                Separator();

                Title("Preview");
                Message("Avatar Bone Preview", _target._avatar.NamingConvention.PreviewText);
                Message("Cloth Bone Preview", _target._cloth.NamingConvention.PreviewText);
            }

            private static void TextField(string label, ref string value)
            {
                HorizontalField(label, o => EditorGUILayout.TextField(o), ref value);
            }

            private static void EnumPopup<T>(string label, ref T value) where T : Enum
            {
                HorizontalField(label, o => (T)EditorGUILayout.EnumPopup(o), ref value);
            }

            private static void ObjectField<T>(string label, [CanBeNull] ref T value) where T : UnityEngine.Object
            {
                HorizontalField(label, o => EditorGUILayout.ObjectField(o, typeof(T), true) as T, ref value);
            }

            private static void HorizontalField<T>(string label, [NotNull] Func<T, T> func, ref T param)
            {
                EditorGUILayout.BeginHorizontal();
                Label(label);
                RecordUndo();
                param = func(param);
                EditorGUILayout.EndHorizontal();
            }

            private static void RecordUndo()
            {
                Undo.RecordObject(_target, "Auto Dresser");
                PrefabUtility.RecordPrefabInstancePropertyModifications(_target);
            }

            private static void Label(string label)
            {
                var width = EditorStyles.label.CalcSize(new GUIContent(label)).x;
                EditorGUILayout.LabelField(label, GUILayout.Width(width + 5));
            }

            private static void Separator()
            {
                EditorGUILayout.Space();
                DrawHorizontalLine(1, Color.black);
                EditorGUILayout.Space();
            }

            private static void DrawHorizontalLine(int thickness, Color color)
            {
                var rect = EditorGUILayout.GetControlRect(false, thickness);
                EditorGUI.DrawRect(rect, color);
            }

            private static void TitleSub(string title)
            {
                Title(title, 15);
            }

            private static void Title(string title, int fontSize = 20)
            {
                var style = new GUIStyle(EditorStyles.boldLabel) { fontSize = fontSize };
                EditorGUILayout.LabelField(title, style);
                EditorGUILayout.Space();
            }

            private static void MessageBold(string title, string message)
            {
                EditorGUILayout.LabelField(title, message, EditorStyles.boldLabel);
            }

            private static void MessageBold(string message)
            {
                EditorGUILayout.LabelField(message, EditorStyles.boldLabel);
            }

            private static void Message(string title, string message)
            {
                EditorGUILayout.LabelField(title, message, EditorStyles.label);
            }

            private static void Message(string message)
            {
                EditorGUILayout.LabelField(message, EditorStyles.label);
            }

            private static void Info(string message)
            {
                EditorGUILayout.HelpBox(message, MessageType.Info);
            }

            private static void Warning(string message)
            {
                EditorGUILayout.HelpBox(message, MessageType.Warning);
            }

            private static void Error(string message)
            {
                EditorGUILayout.HelpBox(message, MessageType.Error);
            }
        }
    }
}
#endif
