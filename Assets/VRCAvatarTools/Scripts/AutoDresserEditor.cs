#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Rekorn.VRCAvatarTools
{
    [CustomEditor(typeof(AutoDresser))]
    public sealed class AutoDresserEditor : Editor
    {
        private AutoDresser _target;

        private void OnEnable()
        {
            _target = (AutoDresser)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Change Naming Convention"))
            {
                _target.ChangeNamingConvention();
            }
            if (GUILayout.Button("Apply Cloth"))
            {
                _target.ApplyCloth();
            }
        }
    }
}
#endif
