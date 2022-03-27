using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(List<>),           true)]
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : PropertyDrawer
    {
        // Do not make this static, it will cause key collisions of cache dictionary.
        readonly ReorderableListProperty _list = new ReorderableListProperty(SerializedList.ListName);

        PropertyDrawerAction _draw;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (_draw == null) _draw = DrawProperty;
            property.OnGUIProperty(rect, label, _draw);
        }

        void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            _list.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            _list.Update(property).GetHeight();
    }
}
