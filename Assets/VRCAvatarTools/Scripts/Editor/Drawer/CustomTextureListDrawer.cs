using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    //[CustomPropertyDrawer(typeof(TexturePropertyMap))]
    public class CustomTextureListDrawer : PropertyDrawer
    {
        PropertyDrawerAction _draw;

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (_draw == null) _draw = DrawProperty;
            property.OnGUIProperty(rect, label, _draw);
        }

        void DrawProperty(Rect rect, SerializedProperty property, GUIContent _)
        {
            var typeProp = property.FindPropertyRelative("Type");
            var texturesProp = property.FindPropertyRelative("Textures");

            rect.height = 0f;

            rect.AppendHeight(typeProp);
            typeProp.SimpleDisabledPropertyField(rect);

            rect.AppendHeight(texturesProp);
            texturesProp.SimplePropertyField(rect);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent _)
        {
            var typeProp     = property.FindPropertyRelative("Type");
            var texturesProp = property.FindPropertyRelative("Textures");

            return typeProp.GetPropertyHeight() + texturesProp.GetPropertyHeight();
        }
    }
}
