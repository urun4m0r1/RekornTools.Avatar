using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(RigExclusion))]
    public sealed class RigExclusionDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent title, int indent)
        {
            rect.height = 0f;

            var disableParenting = property.ResolveProperty(nameof(RigExclusion.IgnoreParenting));
            var avatarBoneName   = property.ResolveProperty(nameof(RigExclusion.AvatarBone));
            var clothBoneName    = property.ResolveProperty(nameof(RigExclusion.ClothBone));

            rect.AppendHeight(avatarBoneName);

            var x     = rect.x;
            var width = rect.width;
            rect.width = 0f;

            rect.AppendWidth(width * 0.45f);
            clothBoneName.PropertyField(rect);

            rect.AppendWidth(width * 0.1f);
            EditorGUI.LabelField(rect, "▶", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });

            rect.AppendWidth(width * 0.45f);
            avatarBoneName.PropertyField(rect);

            rect.x     = x;
            rect.width = width;

            rect.AppendHeight(disableParenting);
            disableParenting.PropertyField(rect, "Ignore Parenting");
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            EditorGUIExtensions.SingleItemHeight * 2f;
    }
}
