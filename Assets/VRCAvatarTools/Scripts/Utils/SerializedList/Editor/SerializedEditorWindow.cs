using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public abstract class SerializedEditorWindow<T> : EditorWindow
    {
        [NotNull] private static string PrefPath => $"{Application.identifier}/{typeof(T)}";

        private Vector2 _scrollPosition = Vector2.zero;
        private bool    _isSerializedFieldsReady;

        private void OnEnable()
        {
            EditorApplication.quitting += SaveSerializedData;
            LoadSerializedData();
            Enable();
            _isSerializedFieldsReady = true;
        }

        private void OnDisable()
        {
            EditorApplication.quitting -= SaveSerializedData;
            SaveSerializedData();
            Disable();
            _isSerializedFieldsReady = false;
        }

        private void LoadSerializedData()
        {
            var data = EditorPrefs.GetString(PrefPath, EditorJsonUtility.ToJson(this, false));
            EditorJsonUtility.FromJsonOverwrite(data, this);
        }

        private void SaveSerializedData()
        {
            var data = EditorJsonUtility.ToJson(this, false);
            EditorPrefs.SetString(PrefPath, data);
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
        }

        protected abstract void Draw();
    }
}
