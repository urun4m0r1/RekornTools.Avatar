using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : PropertyDrawer
    {
        readonly ReorderableListHelper _helper = new ReorderableListHelper("list");

        public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, prop);
            {
                _helper.Draw(rect, prop);
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent _) => _helper.GetHeight(prop);
    }
}
