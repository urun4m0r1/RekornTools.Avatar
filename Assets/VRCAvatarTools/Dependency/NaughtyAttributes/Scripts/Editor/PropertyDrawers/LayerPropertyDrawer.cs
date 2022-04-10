using UnityEngine;
using UnityEditor;
using System;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	public class LayerPropertyDrawer : PropertyDrawerBase
	{
		private const string TypeWarningMessage = "{0} must be an int or a string";

		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			var validPropertyType = property.propertyType == SerializedPropertyType.String || property.propertyType == SerializedPropertyType.Integer;

			return validPropertyType
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			switch (property.propertyType)
			{
				case SerializedPropertyType.String:
					DrawPropertyForString(rect, property, label, GetLayers());
					break;
				case SerializedPropertyType.Integer:
					DrawPropertyForInt(rect, property, label, GetLayers());
					break;
				default:
					var message = string.Format(TypeWarningMessage, property.name);
					DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
					break;
			}

			EditorGUI.EndProperty();
		}

		private string[] GetLayers()
		{
			return UnityEditorInternal.InternalEditorUtility.layers;
		}

		private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, string[] layers)
		{
			var index = IndexOf(layers, property.stringValue);
			var newIndex = EditorGUI.Popup(rect, label.text, index, layers);
			var newLayer = layers[newIndex];

			if (!property.stringValue.Equals(newLayer, StringComparison.Ordinal))
			{
				property.stringValue = layers[newIndex];
			}
		}

		private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, string[] layers)
		{
			var index = 0;
			var layerName = LayerMask.LayerToName(property.intValue);
			for (var i = 0; i < layers.Length; i++)
			{
				if (layerName.Equals(layers[i], StringComparison.Ordinal))
				{
					index = i;
					break;
				}
			}

			var newIndex = EditorGUI.Popup(rect, label.text, index, layers);
			var newLayerName = layers[newIndex];
			var newLayerNumber = LayerMask.NameToLayer(newLayerName);

			if (property.intValue != newLayerNumber)
			{
				property.intValue = newLayerNumber;
			}
		}

		private static int IndexOf(string[] layers, string layer)
		{
			var index = Array.IndexOf(layers, layer);
			return Mathf.Clamp(index, 0, layers.Length - 1);
		}
	}
}
