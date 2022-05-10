using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(AvatarRig<>), true)]
    public sealed class AvatarRigDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent title, int indent)
        {
            rect.height = 0f;

            var rig    = property.ResolveProperty(nameof(AvatarRig.Rig));
            var naming = property.ResolveProperty(nameof(AvatarRig.Naming));

            rect.AppendHeight(rig);
            rig.PropertyField(rect, "Parent Bone");
            rect.AppendHeight(naming);
            naming.PropertyField(rect);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            EditorGUIExtensions.SingleItemHeight * 3f;
    }
}
