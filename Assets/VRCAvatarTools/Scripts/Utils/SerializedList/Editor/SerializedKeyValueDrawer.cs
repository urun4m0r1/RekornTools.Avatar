using UnityEditor;
using UnityEngine;
using static VRCAvatarTools.SerializedKeyValue;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(SerializedKeyValue<,>), true)]
    public class SerializedKeyValueDrawer : SerializedPropertyDrawer
    {
        protected static readonly SerializedKeyValueHelper Helper = new SerializedKeyValueHelper(KeyField, ValueField);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _) =>
            Helper?.Update(property).DrawVertical(rect, true);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            Helper?.Update(property).TotalHeight ?? 0f;
    }
}
