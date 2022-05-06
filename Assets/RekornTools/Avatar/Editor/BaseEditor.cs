using JetBrains.Annotations;
using UnityEditor;

namespace RekornTools.Avatar.Editor
{
    public abstract class BaseEditor<T> : UnityEditor.Editor where T : UnityEngine.Object, IValidate
    {
        T _target;

        void OnEnable() => _target = target as T;

        public override void OnInspectorGUI()
        {
            if (_target == null) return;

            Undo.RecordObject(_target, nameof(T));
            {
                Draw(_target);
                if (_target != null) _target.OnValidate();
            }
        }

        protected virtual void Draw([NotNull] T t) { }
    }
}
