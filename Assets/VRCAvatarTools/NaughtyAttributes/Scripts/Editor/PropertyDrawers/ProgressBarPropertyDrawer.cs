using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ProgressBarAttribute))]
	public class ProgressBarPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			var progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
			var maxValue = GetMaxValue(property, progressBarAttribute);

			return IsNumber(property) && IsNumber(maxValue)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			if (!IsNumber(property))
			{
				var message = string.Format("Field {0} is not a number", property.name);
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
				return;
			}

			var progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
			var value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
			var valueFormatted = property.propertyType == SerializedPropertyType.Integer ? value.ToString() : string.Format("{0:0.00}", value);
			var maxValue = GetMaxValue(property, progressBarAttribute);

			if (maxValue != null && IsNumber(maxValue))
			{
				var fillPercentage = value / CastToFloat(maxValue);
				var barLabel = (!string.IsNullOrEmpty(progressBarAttribute.Name) ? "[" + progressBarAttribute.Name + "] " : "") + valueFormatted + "/" + maxValue;
				var barColor = progressBarAttribute.Color.GetColor();
				var labelColor = Color.white;

				var indentLength = NaughtyEditorGUI.GetIndentLength(rect);
				var barRect = new Rect()
				{
					x = rect.x + indentLength,
					y = rect.y,
					width = rect.width - indentLength,
					height = EditorGUIUtility.singleLineHeight
				};

				DrawBar(barRect, Mathf.Clamp01(fillPercentage), barLabel, barColor, labelColor);
			}
			else
			{
				var message = string.Format(
					"The provided dynamic max value for the progress bar is not correct. Please check if the '{0}' is correct, or the return type is float/int",
					nameof(progressBarAttribute.MaxValueName));

				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private object GetMaxValue(SerializedProperty property, ProgressBarAttribute progressBarAttribute)
		{
			if (string.IsNullOrEmpty(progressBarAttribute.MaxValueName))
			{
				return progressBarAttribute.MaxValue;
			}
			else
			{
				var target = PropertyUtility.GetTargetObjectWithProperty(property);

				var valuesFieldInfo = ReflectionUtility.GetField(target, progressBarAttribute.MaxValueName);
				if (valuesFieldInfo != null)
				{
					return valuesFieldInfo.GetValue(target);
				}

				var valuesPropertyInfo = ReflectionUtility.GetProperty(target, progressBarAttribute.MaxValueName);
				if (valuesPropertyInfo != null)
				{
					return valuesPropertyInfo.GetValue(target);
				}

				var methodValuesInfo = ReflectionUtility.GetMethod(target, progressBarAttribute.MaxValueName);
				if (methodValuesInfo != null &&
					(methodValuesInfo.ReturnType == typeof(float) || methodValuesInfo.ReturnType == typeof(int)) &&
					methodValuesInfo.GetParameters().Length == 0)
				{
					return methodValuesInfo.Invoke(target, null);
				}

				return null;
			}
		}

		private void DrawBar(Rect rect, float fillPercent, string label, Color barColor, Color labelColor)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			var fillRect = new Rect(rect.x, rect.y, rect.width * fillPercent, rect.height);

			EditorGUI.DrawRect(rect, new Color(0.13f, 0.13f, 0.13f));
			EditorGUI.DrawRect(fillRect, barColor);

			// set alignment and cache the default
			var align = GUI.skin.label.alignment;
			GUI.skin.label.alignment = TextAnchor.UpperCenter;

			// set the color and cache the default
			var c = GUI.contentColor;
			GUI.contentColor = labelColor;

			// calculate the position
			var labelRect = new Rect(rect.x, rect.y - 2, rect.width, rect.height);

			// draw~
			EditorGUI.DropShadowLabel(labelRect, label);

			// reset color and alignment
			GUI.contentColor = c;
			GUI.skin.label.alignment = align;
		}

		private bool IsNumber(SerializedProperty property)
		{
			var isNumber = property.propertyType == SerializedPropertyType.Float || property.propertyType == SerializedPropertyType.Integer;
			return isNumber;
		}

		private bool IsNumber(object obj)
		{
			return (obj is float) || (obj is int);
		}

		private float CastToFloat(object obj)
		{
			if (obj is int)
			{
				return (int)obj;
			}
			else
			{
				return (float)obj;
			}
		}
	}
}
