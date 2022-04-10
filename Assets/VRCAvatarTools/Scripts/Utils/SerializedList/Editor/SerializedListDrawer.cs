using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : SerializedPropertyDrawer
    {
        private readonly ReorderableListHelper _list = new ReorderableListHelper(SerializedList.ListName);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            _list.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            _list.Update(property).GetHeight();
    }
}
