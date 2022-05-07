using UnityEditor;
using UnityEngine;

namespace RekornTools.Avatar.Editor
{
    [CustomPropertyDrawer(typeof(ShaderProperty))]
    public sealed class ShaderPropertyDrawer : BasePropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int __)
        {
            var padding   = EditorGUIUtility.standardVerticalSpacing;
            var fullWidth = rect.width;

            rect.width = 0f;

            DrawElement(0.15f, nameof(ShaderProperty.Shader));
            DrawElement(0.1f,  nameof(ShaderProperty.Index));
            DrawElement(0.15f, nameof(ShaderProperty.Type));
            DrawElement(0.4f,  nameof(ShaderProperty.Name));
            DrawElement(0.2f,  nameof(ShaderProperty.TextureType), false);

            void DrawElement(float weight, string propertyName, bool isReadOnly = true)
            {
                if (propertyName == null) return;

                var prop = property.ResolveProperty(propertyName);
                rect.AppendWidth(fullWidth * weight - padding);
                prop.DisabledPropertyField(rect, isReadOnly);
                rect.AppendWidth(padding);
            }
        }
    }
}
