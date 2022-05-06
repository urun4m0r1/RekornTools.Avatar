using JetBrains.Annotations;
using RekornTools.Avatar.Editor;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar
{
    [CustomPropertyDrawer(typeof(SerializedKeyValue<,>), true)]
    public class SerializedKeyValueDrawer : SerializedPropertyDrawer
    {
        [NotNull] protected static readonly SerializedKeyValueHelper Helper = new SerializedKeyValueHelper();

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int __) =>
            Helper.Update(property).DrawVertical(rect, true);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            Helper.Update(property).TotalHeight;
    }
}
