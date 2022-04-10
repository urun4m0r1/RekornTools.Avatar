using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public abstract class SerializedEditorWindow<T> : EditorWindow
    {
        [NotNull] private static string PrefPath => Application.identifier + "/" + typeof(T);

        private Vector2 _scrollPosition = Vector2.zero;
        private bool    _isSerializedFieldsReady;

        private void OnEnable()
        {
            var data = EditorPrefs.GetString(PrefPath, EditorJsonUtility.ToJson(this, false));
            EditorJsonUtility.FromJsonOverwrite(data, this);
            Enable();
            _isSerializedFieldsReady = true;
        }

        private void OnDisable()
        {
            var data = EditorJsonUtility.ToJson(this, false);
            EditorPrefs.SetString(PrefPath, data);
            Disable();
            _isSerializedFieldsReady = false;
        }

        protected virtual void Enable()  { }
        protected virtual void Disable() { }

        public void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            {
                if (_isSerializedFieldsReady) Draw();
            }
            GUILayout.EndScrollView();

            if (GUI.changed)
            {
                Repaint();
                EditorUtility.SetDirty(this);
            }
        }

        protected abstract void Draw();
    }
}
