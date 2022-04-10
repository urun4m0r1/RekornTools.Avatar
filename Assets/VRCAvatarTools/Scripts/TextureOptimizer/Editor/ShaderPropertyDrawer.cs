using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    [CustomPropertyDrawer(typeof(ShaderProperty))]
    public class ShaderPropertyDrawer : SerializedPropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _)
        {
            var shader      = property.ResolveProperty(ShaderProperty.ShaderField);
            var index       = property.ResolveProperty(ShaderProperty.IndexField);
            var name        = property.ResolveProperty(ShaderProperty.NameField);
            var type        = property.ResolveProperty(ShaderProperty.TypeField);
            var textureType = property.ResolveProperty(ShaderProperty.TextureTypeField);

            var w  = rect.width;
            var w1 = w * 0.15f;
            var w2 = w * 0.1f;
            var w3 = w * 0.15f;
            var w4 = w * 0.4f;
            var w5 = w * 0.2f;

            rect.y      += EditorGUIUtility.standardVerticalSpacing;
            rect.height =  EditorGUIUtility.singleLineHeight;

            var padding = EditorGUIUtility.standardVerticalSpacing * 0.5f;

            rect.width = w1 - padding;
            shader.DisabledPropertyField(rect);
            rect.x += w1;

            rect.width = w2 - padding;
            index.DisabledPropertyField(rect);
            rect.x += w2;

            rect.width = w3 - padding;
            type.DisabledPropertyField(rect);
            rect.x += w3;

            rect.width = w4 - padding;
            name.DisabledPropertyField(rect);
            rect.x += w4;

            rect.width = w5;
            textureType.PropertyField(rect);
        }
    }
}
