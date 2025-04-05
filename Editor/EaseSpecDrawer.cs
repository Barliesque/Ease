using UnityEngine;
using UnityEditor;
using Barliesque.InspectorTools.Editor;


namespace Barliesque.Easing.Editor
{

	[CustomPropertyDrawer(typeof(EaseSpec))]
	public class EaseSpecDrawer : PropertyDrawerHelper
	{
		static Material PreviewMaterial;

		override protected int LinesPerElement => 1;

		override public float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 24f;
		}

		override public void CustomDrawer()
		{
			var flipAttr = fieldInfo.GetCustomAttributes(typeof(FlipCurveAttribute), true);
			bool flipped = (flipAttr?.Length > 0);

			Field(40f, 70f, "Style");
			Field(40f, 60f, "Type");

			var style = (EaseStyle)Property.FindPropertyRelative("Style").intValue;
			var type = (EaseType)Property.FindPropertyRelative("Type").intValue;

			// Draw a preview of the curve
			// Ref: https://answers.unity.com/questions/1360515/how-do-i-draw-lines-in-a-custom-inspector.html
			if (!PreviewMaterial)
			{
				PreviewMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
			}

			// Solves a strange extra draw call where _position is misaligned
			if (_position.y < 16f) return;

			//TODO  This is some screwy shit that needs rewriting (one day far in the future)
			const float offset = 34f;
			var rect = new Rect(_position);
			float top = rect.y;
			float left = offset;
			float width = rect.width;
			rect.x -= width + 10f;
			rect.x += 40f;
			rect.y = -1f;
			rect.width += offset;
			width += offset;
			rect.height += 2f;
			
			//Debug.Log($"{top} {left} {width} {_pos} {_position.y}");

			GUI.BeginClip(rect);
			GL.PushMatrix();
			
			PreviewMaterial.SetPass(0);

			// Draw a box
			GL.Begin(GL.QUADS);
			GL.Color(Color.black);
			GL.Vertex3(left, top, 0f);
			GL.Vertex3(width, top, 0f);
			GL.Vertex3(width, top + rect.height, 0f);
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

			width -= offset;

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
			
			GL.PopMatrix();
			GUI.EndClip();
			
		}

	}
}