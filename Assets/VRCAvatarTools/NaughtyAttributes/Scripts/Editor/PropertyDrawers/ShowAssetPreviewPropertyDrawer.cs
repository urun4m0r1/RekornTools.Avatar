using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ShowAssetPreviewAttribute))]
	public class ShowAssetPreviewPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				var previewTexture = GetAssetPreview(property);
				if (previewTexture != null)
				{
					return GetPropertyHeight(property) + GetAssetPreviewSize(property).y;
				}
				else
				{
					return GetPropertyHeight(property);
				}
			}
			else
			{
				return GetPropertyHeight(property) + GetHelpBoxHeight();
			}
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				var propertyRect = new Rect()
				{
					x = rect.x,
					y = rect.y,
					width = rect.width,
					height = EditorGUIUtility.singleLineHeight
				};

				EditorGUI.PropertyField(propertyRect, property, label);

				var previewTexture = GetAssetPreview(property);
				if (previewTexture != null)
				{
					var previewRect = new Rect()
					{
						x = rect.x + NaughtyEditorGUI.GetIndentLength(rect),
						y = rect.y + EditorGUIUtility.singleLineHeight,
						width = rect.width,
						height = GetAssetPreviewSize(property).y
					};

					GUI.Label(previewRect, previewTexture);
				}
			}
			else
			{
				var message = property.name + " doesn't have an asset preview";
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private Texture2D GetAssetPreview(SerializedProperty property)
		{
			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				if (property.objectReferenceValue != null)
				{
					var previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
					return previewTexture;
				}

				return null;
			}

			return null;
		}

		private Vector2 GetAssetPreviewSize(SerializedProperty property)
		{
			var previewTexture = GetAssetPreview(property);
			if (previewTexture == null)
			{
				return Vector2.zero;
			}
			else
			{
				var targetWidth = ShowAssetPreviewAttribute.DefaultWidth;
				var targetHeight = ShowAssetPreviewAttribute.DefaultHeight;

				var showAssetPreviewAttribute = PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);
				if (showAssetPreviewAttribute != null)
				{
					targetWidth = showAssetPreviewAttribute.Width;
					targetHeight = showAssetPreviewAttribute.Height;
				}

				var width = Mathf.Clamp(targetWidth, 0, previewTexture.width);
				var height = Mathf.Clamp(targetHeight, 0, previewTexture.height);

				return new Vector2(width, height);
			}
		}
	}
}
