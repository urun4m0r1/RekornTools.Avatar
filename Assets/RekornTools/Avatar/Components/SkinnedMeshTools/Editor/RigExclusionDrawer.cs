using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(RigPair<>), true)]
    public sealed class RigExclusionDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent title, int indent)
        {
            rect.height = 0f;

            var avatarBoneName = property.ResolveProperty(nameof(RigPair.Avatar));
            var clothBoneName  = property.ResolveProperty(nameof(RigPair.Cloth));

            rect.AppendHeight(avatarBoneName);

            var x     = rect.x;
            var width = rect.width;
            rect.width = 0f;

            rect.AppendWidth(width * 0.45f);
            avatarBoneName.PropertyField(rect);

            rect.AppendWidth(width * 0.1f);
            EditorGUI.LabelField(rect, "◀", new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleCenter });

            rect.AppendWidth(width * 0.45f);
            clothBoneName.PropertyField(rect);

            rect.x     = x;
            rect.width = width;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            EditorGUIExtensions.SingleItemHeight;
    }
}
