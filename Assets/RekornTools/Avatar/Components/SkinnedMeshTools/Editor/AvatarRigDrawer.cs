﻿using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(AvatarRig))]
    public sealed class AvatarRigDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent title, int indent)
        {
            rect.height = 0f;

            var rig    = property.ResolveProperty(nameof(AvatarRig.Rig));
            var naming = property.ResolveProperty(nameof(AvatarRig.Naming));

            rect.AppendHeight(rig);
            rig.PropertyField(rect, $"{title?.text} Parent");
            rect.AppendHeight(naming);
            naming.PropertyField(rect);
        }
    }
}