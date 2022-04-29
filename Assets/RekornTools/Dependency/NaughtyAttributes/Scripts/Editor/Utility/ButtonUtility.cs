using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class ButtonUtility
	{
		public static bool IsEnabled(Object target, MethodInfo method)
		{
			var enableIfAttribute = method.GetCustomAttribute<EnableIfAttributeBase>();
			if (enableIfAttribute == null)
			{
				return true;
			}

			var conditionValues = PropertyUtility.GetConditionValues(target, enableIfAttribute.Conditions);
			if (conditionValues.Count > 0)
			{
				var enabled = PropertyUtility.GetConditionsFlag(conditionValues, enableIfAttribute.ConditionOperator, enableIfAttribute.Inverted);
				return enabled;
			}
			else
			{
				var message = enableIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
				Debug.LogWarning(message, target);

				return false;
			}
		}

		public static bool IsVisible(Object target, MethodInfo method)
		{
			var showIfAttribute = method.GetCustomAttribute<ShowIfAttributeBase>();
			if (showIfAttribute == null)
			{
				return true;
			}

			var conditionValues = PropertyUtility.GetConditionValues(target, showIfAttribute.Conditions);
			if (conditionValues.Count > 0)
			{
				var enabled = PropertyUtility.GetConditionsFlag(conditionValues, showIfAttribute.ConditionOperator, showIfAttribute.Inverted);
				return enabled;
			}
			else
			{
				var message = showIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
				Debug.LogWarning(message, target);

				return false;
			}
		}
	}
}
