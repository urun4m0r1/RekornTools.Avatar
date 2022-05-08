using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(RigNamePair))]
    public sealed class RigNamePairDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent title, int indent)
        {
            rect.height = 0f;

            var disableParenting = property.ResolveProperty(nameof(RigNamePair.DisableParenting));
            var avatarBoneName   = property.ResolveProperty(nameof(RigNamePair.AvatarBoneName));
            var clothBoneName    = property.ResolveProperty(nameof(RigNamePair.ClothBoneName));

            rect.AppendHeight(disableParenting);
            disableParenting.PropertyField(rect, "Disable Parenting");
            rect.AppendHeight(avatarBoneName);
            avatarBoneName.PropertyField(rect, "Avatar Bone Name");
            rect.AppendHeight(clothBoneName);
            clothBoneName.PropertyField(rect, "Cloth Bone Name");
        }
    }
}
