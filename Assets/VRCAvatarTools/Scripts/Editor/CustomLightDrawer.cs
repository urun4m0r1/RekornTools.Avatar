using UnityEditor;
using UnityEngine;

namespace VRCAvatarTools
{
    [CustomPropertyDrawer(typeof(CustomLight))]
    public class CustomLightDrawer : UnityEditor.PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                EditorGUILayout.LabelField(property.displayName, EditorStyles.boldLabel);

                var sunDirection = property.FindPropertyRelative("sunDirection");

                var direction = EditorGUILayout.Vector3Field("Sun Direction", sunDirection.vector3Value);

                direction.x = Mathf.Clamp(direction.x, -1f, 1f);
                direction.y = Mathf.Clamp(direction.y, -1f, 1f);
                direction.z = Mathf.Clamp(direction.z, -1f, 1f);

                sunDirection.vector3Value = direction;

                EditorGUILayout.BeginHorizontal();
                {
                    var sunIntensity = property.FindPropertyRelative("sunIntensity");
                    var sunColor     = property.FindPropertyRelative("sunColor");

                    EditorGUILayout.LabelField("Sun", GUILayout.Width(40));
                    sunIntensity.floatValue = EditorGUILayout.Slider(sunIntensity.floatValue, 0f, 8f);
                    sunColor.colorValue     = EditorGUILayout.ColorField(sunColor.colorValue, GUILayout.Width(60));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    var skyboxIntensity = property.FindPropertyRelative("skyboxIntensity");
                    var skyColor        = property.FindPropertyRelative("skyColor");

                    EditorGUILayout.LabelField("Sky", GUILayout.Width(40));
                    skyboxIntensity.floatValue = EditorGUILayout.Slider(skyboxIntensity.floatValue, 0f, 8f);

                    if (!CustomLight.UseSkybox)
                    {
                        skyColor.colorValue = EditorGUILayout.ColorField(skyColor.colorValue, GUILayout.Width(60));
                    }
                    else
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        {
                            EditorGUILayout.ColorField(Color.clear, GUILayout.Width(60));
                        }
                        EditorGUI.EndDisabledGroup();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndProperty();
        }
    }
}
