﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public class ReorderableListPropertyDrawer : SpecialCasePropertyDrawerBase
	{
		public static readonly ReorderableListPropertyDrawer Instance = new ReorderableListPropertyDrawer();

		private readonly Dictionary<string, ReorderableList> _reorderableListsByPropertyName = new Dictionary<string, ReorderableList>();

		private GUIStyle _labelStyle;

		private GUIStyle GetLabelStyle()
		{
			if (_labelStyle == null)
			{
				_labelStyle = new GUIStyle(EditorStyles.boldLabel);
				_labelStyle.richText = true;
			}

			return _labelStyle;
		}

		private string GetPropertyKeyName(SerializedProperty property)
		{
			return property.serializedObject.targetObject.GetInstanceID() + "." + property.name;
		}

		protected override float GetPropertyHeight_Internal(SerializedProperty property)
		{
			if (property.isArray)
			{
				var key = GetPropertyKeyName(property);

				if (_reorderableListsByPropertyName.TryGetValue(key, out var reorderableList) == false)
				{
					return 0;
				}

				return reorderableList.GetHeight();
			}

			return EditorGUI.GetPropertyHeight(property, true);
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			if (property.isArray)
			{
				var key = GetPropertyKeyName(property);

				ReorderableList reorderableList = null;
				if (!_reorderableListsByPropertyName.ContainsKey(key))
				{
					reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
					{
						drawHeaderCallback = (Rect r) =>
						{
							EditorGUI.LabelField(r, string.Format("{0}: {1}", label.text, property.arraySize), GetLabelStyle());
							HandleDragAndDrop(r, reorderableList);
						},

						drawElementCallback = (Rect r, int index, bool isActive, bool isFocused) =>
						{
							var element = property.GetArrayElementAtIndex(index);
							r.y += 1.0f;
							r.x += 10.0f;
							r.width -= 10.0f;

							EditorGUI.PropertyField(new Rect(r.x, r.y, r.width, EditorGUIUtility.singleLineHeight), element, true);
						},

						elementHeightCallback = (int index) =>
						{
							return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;
						}
					};

					_reorderableListsByPropertyName[key] = reorderableList;
				}

				reorderableList = _reorderableListsByPropertyName[key];

				if (rect == default)
				{
					reorderableList.DoLayoutList();
				}
				else
				{
					reorderableList.DoList(rect);
				}
			}
			else
			{
				var message = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
				NaughtyEditorGUI.HelpBox_Layout(message, MessageType.Warning, context: property.serializedObject.targetObject);
				EditorGUILayout.PropertyField(property, true);
			}
		}

		public void ClearCache()
		{
			_reorderableListsByPropertyName.Clear();
		}

		private Object GetAssignableObject(Object obj, ReorderableList list)
		{
			var listType = PropertyUtility.GetPropertyType(list.serializedProperty);
			var elementType = ReflectionUtility.GetListElementType(listType);

			if (elementType == null)
			{
				return null;
			}

			var objType = obj.GetType();

			if (elementType.IsAssignableFrom(objType))
			{
				return obj;
			}

			if (objType == typeof(GameObject))
			{
				if (typeof(Transform).IsAssignableFrom(elementType))
				{
					var transform = ((GameObject)obj).transform;
					if (elementType == typeof(RectTransform))
					{
						var rectTransform = transform as RectTransform;
						return rectTransform;
					}
					else
					{
						return transform;
					}
				}
				else if (typeof(MonoBehaviour).IsAssignableFrom(elementType))
				{
					return ((GameObject)obj).GetComponent(elementType);
				}
			}

			return null;
		}

		private void HandleDragAndDrop(Rect rect, ReorderableList list)
		{
			var currentEvent = Event.current;
			var usedEvent = false;

			switch (currentEvent.type)
			{
				case EventType.DragExited:
					if (GUI.enabled)
					{
						HandleUtility.Repaint();
					}

					break;

				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (rect.Contains(currentEvent.mousePosition) && GUI.enabled)
					{
						// Check each single object, so we can add multiple objects in a single drag.
						var didAcceptDrag = false;
						var references = DragAndDrop.objectReferences;
						foreach (var obj in references)
						{
							var assignableObject = GetAssignableObject(obj, list);
							if (assignableObject != null)
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
								if (currentEvent.type == EventType.DragPerform)
								{
									list.serializedProperty.arraySize++;
									var arrayEnd = list.serializedProperty.arraySize - 1;
									list.serializedProperty.GetArrayElementAtIndex(arrayEnd).objectReferenceValue = assignableObject;
									didAcceptDrag = true;
								}
							}
						}

						if (didAcceptDrag)
						{
							GUI.changed = true;
							DragAndDrop.AcceptDrag();
							usedEvent = true;
						}
					}

					break;
			}

			if (usedEvent)
			{
				currentEvent.Use();
			}
		}
	}
}
