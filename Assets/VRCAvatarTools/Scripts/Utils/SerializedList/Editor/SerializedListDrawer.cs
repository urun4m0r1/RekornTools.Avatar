using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(List<>),           true)]
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : SerializedPropertyDrawer
    {
        // Do not make this static, it will cause key collisions of cache dictionary.
        readonly ReorderableListProperty _list = new ReorderableListProperty(SerializedList.ListName);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            _list.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            _list.Update(property).GetHeight();
    }
}
