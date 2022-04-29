using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [CustomPropertyDrawer(typeof(SerializedList<>), true)]
    public class SerializedListDrawer : SerializedPropertyDrawer
    {
        [NotNull] protected readonly ReorderableListHelper Helper = new ReorderableListHelper(SerializedList.FieldName);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int __) =>
            Helper.Update(property).Draw(rect);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            Helper.Update(property).GetHeight();
    }
}
