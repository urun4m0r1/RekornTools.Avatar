using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    [CustomPropertyDrawer(typeof(ShaderProperty))]
    public class ShaderPropertyDrawer : SerializedPropertyDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _, int __)
        {
            var padding   = EditorGUIUtility.standardVerticalSpacing;
            var fullWidth = rect.width;

            rect.width = 0f;

            DrawElement(0.15f, ShaderProperty.ShaderField);
            DrawElement(0.1f,  ShaderProperty.IndexField);
            DrawElement(0.15f, ShaderProperty.TypeField);
            DrawElement(0.4f,  ShaderProperty.NameField);
            DrawElement(0.2f,  ShaderProperty.TextureTypeField, false);

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
