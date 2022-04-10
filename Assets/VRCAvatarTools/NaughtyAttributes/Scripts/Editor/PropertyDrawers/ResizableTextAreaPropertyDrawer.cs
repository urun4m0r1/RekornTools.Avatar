using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ResizableTextAreaAttribute))]
	public class ResizableTextAreaPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String)
			{
				var labelHeight = EditorGUIUtility.singleLineHeight;
				var textAreaHeight = GetTextAreaHeight(property.stringValue);
				return labelHeight + textAreaHeight;
			}
			else
			{
				return GetPropertyHeight(property) + GetHelpBoxHeight();
			}
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			if (property.propertyType == SerializedPropertyType.String)
			{
				var labelRect = new Rect()
				{
					x = rect.x,
					y = rect.y,
					width = rect.width,
					height = EditorGUIUtility.singleLineHeight
				};

				EditorGUI.LabelField(labelRect, label.text);

				EditorGUI.BeginChangeCheck();

				var textAreaRect = new Rect()
				{
					x = labelRect.x,
					y = labelRect.y + EditorGUIUtility.singleLineHeight,
					width = labelRect.width,
					height = GetTextAreaHeight(property.stringValue)
				};

				var textAreaValue = EditorGUI.TextArea(textAreaRect, property.stringValue);

				if (EditorGUI.EndChangeCheck())
				{
					property.stringValue = textAreaValue;
				}
			}
			else
			{
				var message = typeof(ResizableTextAreaAttribute).Name + " can only be used on string fields";
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private int GetNumberOfLines(string text)
		{
			var content = Regex.Replace(text, @"\r\n|\n\r|\r|\n", Environment.NewLine);
			var lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			return lines.Length;
		}

		private float GetTextAreaHeight(string text)
		{
			var height = (EditorGUIUtility.singleLineHeight - 3.0f) * GetNumberOfLines(text) + 3.0f;
			return height;
		}
	}
}
