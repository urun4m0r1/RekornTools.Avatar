﻿using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    public abstract class BaseEditorWindow<T> : EditorWindow
    {
        [NotNull] static string PrefPath => $"{Application.identifier}/{typeof(T)}";

        Vector2 _scrollPosition = Vector2.zero;
        bool    _isSerializedFieldsReady;

        void OnEnable()
        {
            EditorApplication.quitting += SaveSerializedData;
            LoadSerializedData();
            Enable();
            _isSerializedFieldsReady = true;
        }

        void OnDisable()
        {
            EditorApplication.quitting -= SaveSerializedData;
            SaveSerializedData();
            Disable();
            _isSerializedFieldsReady = false;
        }

        void LoadSerializedData()
        {
            var data = EditorPrefs.GetString(PrefPath, EditorJsonUtility.ToJson(this, false));
            EditorJsonUtility.FromJsonOverwrite(data, this);
        }

        void SaveSerializedData()
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
                Undo.RecordObject(this, typeof(T).Name);
                {
                    if (_isSerializedFieldsReady) Draw();
                }
                Repaint();
            }
            GUILayout.EndScrollView();
        }

        protected abstract void Draw();
    }
}
