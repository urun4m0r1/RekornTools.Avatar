using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools.Editor
{
    [CustomPropertyDrawer(typeof(TypedTexturesMap))]
    public class CustomTextureListDrawer : SerializedKeyValueDrawer
    {
        protected override void DrawProperty(Rect rect, SerializedProperty property, GUIContent _)
        {
            var typeProp = property.FindPropertyRelative("Type");
            var texturesProp = property.FindPropertyRelative("Textures");

            rect.height = 0f;

            rect.AppendHeight(typeProp);
            typeProp.DisabledPropertyField(rect);

            rect.AppendHeight(texturesProp);
            texturesProp.PropertyField(rect);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _)
        {
            var typeProp     = property.FindPropertyRelative("Type");
            var texturesProp = property.FindPropertyRelative("Textures");

            return typeProp.GetHeight() + texturesProp.GetHeight();
        }
    }
}
