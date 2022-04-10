using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : SerializedPropertyDrawer
    {
        [NotNull] protected readonly ReorderableListHelper Helper = new ReorderableListHelper(SerializedList.ListName);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            Helper.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            Helper.Update(property).GetHeight();
    }
}
