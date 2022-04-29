using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(AnimatorParamAttribute))]
	public class AnimatorParamPropertyDrawer : PropertyDrawerBase
	{
		private const string InvalidAnimatorControllerWarningMessage = "Target animator controller is null";
		private const string InvalidTypeWarningMessage = "{0} must be an int or a string";

		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			var animatorParamAttribute = PropertyUtility.GetAttribute<AnimatorParamAttribute>(property);
			var validAnimatorController = GetAnimatorController(property, animatorParamAttribute.AnimatorName) != null;
			var validPropertyType = property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.String;

			return (validAnimatorController && validPropertyType)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			var animatorParamAttribute = PropertyUtility.GetAttribute<AnimatorParamAttribute>(property);

			var animatorController = GetAnimatorController(property, animatorParamAttribute.AnimatorName);
			if (animatorController == null)
			{
				DrawDefaultPropertyAndHelpBox(rect, property, InvalidAnimatorControllerWarningMessage, MessageType.Warning);
				return;
			}

			var parametersCount = animatorController.parameters.Length;
			var animatorParameters = new List<AnimatorControllerParameter>(parametersCount);
			for (var i = 0; i < parametersCount; i++)
			{
				var parameter = animatorController.parameters[i];
				if (animatorParamAttribute.AnimatorParamType == null || parameter.type == animatorParamAttribute.AnimatorParamType)
				{
					animatorParameters.Add(parameter);
				}
			}

			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					DrawPropertyForInt(rect, property, label, animatorParameters);
					break;
				case SerializedPropertyType.String:
					DrawPropertyForString(rect, property, label, animatorParameters);
					break;
				default:
					DrawDefaultPropertyAndHelpBox(rect, property, string.Format(InvalidTypeWarningMessage, property.name), MessageType.Warning);
					break;
			}

			EditorGUI.EndProperty();
		}

		private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, List<AnimatorControllerParameter> animatorParameters)
		{
			var paramNameHash = property.intValue;
			var index = 0;

			for (var i = 0; i < animatorParameters.Count; i++)
			{
				if (paramNameHash == animatorParameters[i].nameHash)
				{
					index = i + 1; // +1 because the first option is reserved for (None)
					break;
				}
			}

			var displayOptions = GetDisplayOptions(animatorParameters);

			var newIndex = EditorGUI.Popup(rect, label.text, index, displayOptions);
			var newValue = newIndex == 0 ? 0 : animatorParameters[newIndex - 1].nameHash;

			if (property.intValue != newValue)
			{
				property.intValue = newValue;
			}
		}

		private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, List<AnimatorControllerParameter> animatorParameters)
		{
			var paramName = property.stringValue;
			var index = 0;

			for (var i = 0; i < animatorParameters.Count; i++)
			{
				if (paramName.Equals(animatorParameters[i].name, System.StringComparison.Ordinal))
				{
					index = i + 1; // +1 because the first option is reserved for (None)
					break;
				}
			}

			var displayOptions = GetDisplayOptions(animatorParameters);

			var newIndex = EditorGUI.Popup(rect, label.text, index, displayOptions);
			var newValue = newIndex == 0 ? null : animatorParameters[newIndex - 1].name;

			if (!property.stringValue.Equals(newValue, System.StringComparison.Ordinal))
			{
				property.stringValue = newValue;
			}
		}

		private static string[] GetDisplayOptions(List<AnimatorControllerParameter> animatorParams)
		{
			var displayOptions = new string[animatorParams.Count + 1];
			displayOptions[0] = "(None)";

			for (var i = 0; i < animatorParams.Count; i++)
			{
				displayOptions[i + 1] = animatorParams[i].name;
			}

			return displayOptions;
		}

		private static AnimatorController GetAnimatorController(SerializedProperty property, string animatorName)
		{
			var target = PropertyUtility.GetTargetObjectWithProperty(property);

			var animatorFieldInfo = ReflectionUtility.GetField(target, animatorName);
			if (animatorFieldInfo != null &&
				animatorFieldInfo.FieldType == typeof(Animator))
			{
				var animator = animatorFieldInfo.GetValue(target) as Animator;
				if (animator != null)
				{
					var animatorController = animator.runtimeAnimatorController as AnimatorController;
					return animatorController;
				}
			}

			var animatorPropertyInfo = ReflectionUtility.GetProperty(target, animatorName);
			if (animatorPropertyInfo != null &&
				animatorPropertyInfo.PropertyType == typeof(Animator))
			{
				var animator = animatorPropertyInfo.GetValue(target) as Animator;
				if (animator != null)
				{
					var animatorController = animator.runtimeAnimatorController as AnimatorController;
					return animatorController;
				}
			}

			var animatorGetterMethodInfo = ReflectionUtility.GetMethod(target, animatorName);
			if (animatorGetterMethodInfo != null &&
				animatorGetterMethodInfo.ReturnType == typeof(Animator) &&
				animatorGetterMethodInfo.GetParameters().Length == 0)
			{
				var animator = animatorGetterMethodInfo.Invoke(target, null) as Animator;
				if (animator != null)
				{
					var animatorController = animator.runtimeAnimatorController as AnimatorController;
					return animatorController;
				}
			}

			return null;
		}
	}
}
