using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(RigExclusion))]
    public sealed class RigNamePairDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent title, int indent)
        {
            rect.height = 0f;

            var disableParenting = property.ResolveProperty(nameof(RigExclusion.DisableParenting));
            var avatarBoneName   = property.ResolveProperty(nameof(RigExclusion.AvatarBone));
            var clothBoneName    = property.ResolveProperty(nameof(RigExclusion.ClothBone));

            rect.AppendHeight(disableParenting);
            disableParenting.PropertyField(rect, "Disable Parenting");
            rect.AppendHeight(avatarBoneName);
            avatarBoneName.PropertyField(rect, "Avatar Bone Name");
            rect.AppendHeight(clothBoneName);
            clothBoneName.PropertyField(rect, "Cloth Bone Name");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            EditorGUIExtensions.SingleItemHeight * 3;
    }
}
