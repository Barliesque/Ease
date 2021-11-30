using UnityEngine;
using UnityEditor;
using Barliesque.InspectorTools.Editor;


namespace Barliesque.Ease.Editor
{

	[CustomPropertyDrawer(typeof(EaseSpec))]
	public class EaseSpecDrawer : PropertyDrawerHelper
	{
		static Material PreviewMaterial;

		override public float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 24f;
		}

		override public void CustomDrawer()
		{
			var flipAttr = fieldInfo.GetCustomAttributes(typeof(FlipCurveAttribute), true);
			bool flipped = (flipAttr?.Length > 0);

			Field(44f, 80f, "Style");
			Field(40f, 70f, "Type");

			var style = (EaseStyle)Property.FindPropertyRelative("Style").intValue;
			var type = (EaseType)Property.FindPropertyRelative("Type").intValue;

			// Draw a preview of the curve
			// Ref: https://answers.unity.com/questions/1360515/how-do-i-draw-lines-in-a-custom-inspector.html
			if (PreviewMaterial == null)
			{
				PreviewMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			}

			Rect rect;
			try
			{
				rect = GUILayoutUtility.GetRect(10, 1000, 50, 50);
			}
			catch
			{
				return;
			}

			float top = rect.y - 4;
			float left = Margin;
			float width = rect.width - left;
			rect.x = 0f;
			rect.y = 0f;

			if (Event.current.type == EventType.Repaint)
			{
				GUI.BeginClip(rect);
				GL.PushMatrix();
				GL.Clear(true, false, Color.black);
				PreviewMaterial.SetPass(0);

				// Draw a box
				GL.Begin(GL.QUADS);
				GL.Color(Color.black);
				GL.Vertex3(left, top, 0f);
				GL.Vertex3(rect.width, top, 0f);
				GL.Vertex3(rect.width, top + rect.height, 0f);
				GL.Vertex3(left, top + rect.height, 0f);
				GL.End();
				GL.Begin(GL.LINE_STRIP);
				GL.Color(Color.gray);
				GL.Vertex3(left, top, 0f);
				GL.Vertex3(rect.width, top, 0f);
				GL.Vertex3(rect.width, top + rect.height, 0f);
				GL.Vertex3(left, top + rect.height, 0f);
				GL.Vertex3(left, top, 0f);
				GL.End();

				// Plot the curve
				GL.Begin(GL.LINE_STRIP);
				GL.Color(Color.white);

				for (int x = 0; x < 100; x++)
				{
					var t = (x / 99f);
					Vector2 pos;
					if (flipped)
					{
						pos = new Vector2(left + t * width, top + rect.height - (1f - Ease.Call(style, type, t)) * rect.height);
					}
					else
					{
						pos = new Vector2(left + t * width, top + rect.height - Ease.Call(style, type, t) * rect.height);
					}
					GL.Vertex3(pos.x, pos.y, 0);
				}
				GL.End();
				GUI.EndClip();
				GL.PopMatrix();
			}
		}

	}
}