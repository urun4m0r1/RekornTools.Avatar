using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(InfoBoxAttribute))]
	public class InfoBoxDecoratorDrawer : DecoratorDrawer
	{
		public override float GetHeight()
		{
			return GetHelpBoxHeight();
		}

		public override void OnGUI(Rect rect)
		{
			var infoBoxAttribute = (InfoBoxAttribute)attribute;

			var indentLength = NaughtyEditorGUI.GetIndentLength(rect);
			var infoBoxRect = new Rect(
				rect.x + indentLength,
				rect.y,
				rect.width - indentLength,
				GetHelpBoxHeight());

			DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
		}

		private float GetHelpBoxHeight()
		{
			var infoBoxAttribute = (InfoBoxAttribute)attribute;
			var minHeight = EditorGUIUtility.singleLineHeight * 2.0f;
			var desiredHeight = GUI.skin.box.CalcHeight(new GUIContent(infoBoxAttribute.Text), EditorGUIUtility.currentViewWidth);
			var height = Mathf.Max(minHeight, desiredHeight);

			return height;
		}

		private void DrawInfoBox(Rect rect, string infoText, EInfoBoxType infoBoxType)
		{
			var messageType = MessageType.None;
			switch (infoBoxType)
			{
				case EInfoBoxType.Normal:
					messageType = MessageType.Info;
					break;

				case EInfoBoxType.Warning:
					messageType = MessageType.Warning;
					break;

				case EInfoBoxType.Error:
					messageType = MessageType.Error;
					break;
			}

			NaughtyEditorGUI.HelpBox(rect, infoText, messageType);
		}
	}
}
