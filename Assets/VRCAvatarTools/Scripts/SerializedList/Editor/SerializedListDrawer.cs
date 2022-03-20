using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(ImmutableSerializedList<>), true)]
    public class ImmutableSerializedListDrawer : PropertyDrawer
    {
        readonly ReorderableListHelper _helper = new ReorderableListHelper("list", false, false);

        public override void  OnGUI(Rect rect, SerializedProperty prop, GUIContent _) => _helper.Draw(rect, prop);
        public override float GetPropertyHeight(SerializedProperty prop, GUIContent _) => _helper.GetHeight(prop);
    }

    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : PropertyDrawer
    {
        readonly ReorderableListHelper _helper = new ReorderableListHelper("list");

        public override void  OnGUI(Rect rect, SerializedProperty prop, GUIContent _) => _helper.Draw(rect, prop);
        public override float GetPropertyHeight(SerializedProperty prop, GUIContent _) => _helper.GetHeight(prop);
    }
}
