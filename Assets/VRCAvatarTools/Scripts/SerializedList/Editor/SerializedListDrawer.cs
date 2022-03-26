using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : PropertyDrawer
    {
        static readonly ReorderableListProperty List = new ReorderableListProperty(SerializedList.ListName);
        static readonly PropertyDrawerAction    Draw = DrawProperty;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) =>
            property.OnGUIProperty(position, label, Draw);

        static void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            List.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            List.Update(property).GetHeight();
    }
}
