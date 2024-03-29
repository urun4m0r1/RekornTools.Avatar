﻿using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(SerializedKeyValue<,>))]
    public class SerializedKeyValueDrawer : BasePropertyDrawer
    {
        [NotNull] protected static readonly SerializedKeyValueHelper Helper = new SerializedKeyValueHelper();

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int __) =>
            Helper.Update(property).DrawVertical(rect, true);

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _) =>
            Helper.Update(property).TotalHeight;
    }
}
