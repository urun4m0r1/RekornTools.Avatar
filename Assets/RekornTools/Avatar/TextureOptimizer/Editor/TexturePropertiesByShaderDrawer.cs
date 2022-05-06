using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(TexturePropertiesByShader))]
    public class TexturePropertiesByShaderDrawer : SerializedKeyValueDrawer
    {
        [NotNull] readonly ReorderableListHelper _listHelper = new ReorderableListHelper(SerializedList.FieldName, true);

        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int indent)
        {
            var helper = Helper.Update(property);
            var key    = helper.Key;
            var value  = helper.Value;

            rect.ApplyIndent(++indent);
            var foldoutWidth = 10f;
            var keyWidth     = rect.width - foldoutWidth;

            rect.width  = 0f;
            rect.height = helper.KeyHeight;

            rect.AppendWidth(foldoutWidth);
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, GUIContent.none);

            rect.AppendWidth(keyWidth);
            key.DisabledPropertyField(rect);

            if (property.isExpanded)
            {
                rect.AppendHeight(helper.ValueHeight);
                _listHelper.Update(value).Draw(rect);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _)
        {
            var helper = Helper.Update(property);
            return property?.isExpanded ?? false
                ? helper.ValueHeight
                : helper.KeyHeight;
        }
    }
}
