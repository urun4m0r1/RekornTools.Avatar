﻿using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(RigNamingConvention))]
    public sealed class RigNamingConventionDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int indent)
        {
            var x         = rect.x;
            var fullWidth = rect.width;

            rect.width  = 0f;
            rect.height = 0f;
            rect.AppendHeight(EditorGUIExtensions.SingleItemHeight);

            var type     = property.ResolveProperty(nameof(RigNamingConvention.ModifierPosition));
            var splitter = property.ResolveProperty(nameof(RigNamingConvention.Splitter));
            var left     = property.ResolveProperty(nameof(RigNamingConvention.ModifierLeft));
            var right    = property.ResolveProperty(nameof(RigNamingConvention.ModifierRight));

            DrawElement(0.3f, left);
            DrawElement(0.3f, right);
            DrawElement(0.2f, type);
            DrawElement(0.2f, splitter);

            rect.x     = x;
            rect.width = fullWidth;

            rect.AppendHeight(EditorGUIExtensions.SingleItemHeight);
            EditorGUI.HelpBox(rect, GetPreviewText(), MessageType.Info);

            void DrawElement(float weight, SerializedProperty prop)
            {
                var padding = EditorGUIUtility.standardVerticalSpacing;
                rect.AppendWidth(fullWidth * weight - padding);
                prop.PropertyField(rect);
                rect.AppendWidth(padding);
            }

            string GetPreviewText()
            {
                var t = type?.enumValueIndex;
                var s = splitter?.stringValue;
                var l = left?.stringValue;
                var r = right?.stringValue;

                return t == (int)ModifierPosition.Front
                    ? $"Head / {l}{s}Arm / {r}{s}Leg / {r}{s}Leg.001"
                    : $"Head / Arm{s}{l} / Leg{s}{r} / Leg{s}{r}.001";
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            EditorGUIExtensions.SingleItemHeight * 2;
    }
}
