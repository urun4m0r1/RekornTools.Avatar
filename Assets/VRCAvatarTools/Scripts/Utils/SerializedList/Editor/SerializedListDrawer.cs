using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : SerializedPropertyDrawer
    {
        protected readonly ReorderableListHelper _helper = new ReorderableListHelper(SerializedList.ListName);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            _helper.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            _helper.Update(property).GetHeight();
    }
}
