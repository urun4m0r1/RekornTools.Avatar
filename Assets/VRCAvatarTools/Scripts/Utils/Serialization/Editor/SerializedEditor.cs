using JetBrains.Annotations;
using UnityEditor;

namespace VRCAvatarTools
{
    public abstract class SerializedEditor<T> :
        UnityEditor.Editor
        where T : UnityEngine.Object, IValidate
    {
        private T _target;

        private void OnEnable() => _target = target as T;

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
