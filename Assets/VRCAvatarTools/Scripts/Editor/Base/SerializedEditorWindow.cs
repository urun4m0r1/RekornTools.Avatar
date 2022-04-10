using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    public abstract class SerializedEditorWindow<T> : EditorWindow
    {
        [NotNull] string PrefPath => Application.identifier + "/" + typeof(T).Name;

        bool _isSerializedFieldsReady;

        void OnEnable()
        {
            var data = EditorPrefs.GetString(PrefPath, EditorJsonUtility.ToJson(this, false));
            EditorJsonUtility.FromJsonOverwrite(data, this);
            Enable();
            _isSerializedFieldsReady = true;
        }

        void OnDisable()
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
            if (_isSerializedFieldsReady) Draw();

            if (GUI.changed)
            {
                Repaint();
                EditorUtility.SetDirty(this);
            }
        }

        protected abstract void Draw();
    }
}
