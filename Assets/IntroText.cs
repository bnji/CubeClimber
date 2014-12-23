using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroText : MonoBehaviour
{

		public List<string> textList;
		public float timeForEachText = 2f;
//		public Rect area = new Rect (-7f, 6.56f, 7.5f, -5.3f); 

		private TextMesh textMesh;
		private int index;
		private float lastTime;
		private Color color;
		private float textFadeSpeed = 0.1f;
		private float alpha;

		void Start ()
		{
				textMesh = GetComponent<TextMesh> ();
				color = renderer.material.GetColor ("_Color");
				SetText ();
		}

		void Update ()
		{
				if (index < textList.Count) {
						if (textMesh != null && Time.time - lastTime > timeForEachText) {
								SetText ();
						} else {
								alpha = alpha > 0f ? alpha - textFadeSpeed : alpha;// alpha - Time.deltaTime * timeForEachText * textFadeSpeed : alpha;
								renderer.material.SetColor ("_Color", new Color (color.r, color.g, color.b, alpha));
						}
				}
		}

		void SetText ()
		{
				textMesh.text = textList [index];
//				var size = textMesh.text.Length;
//				var newX = Random.Range (area.x, 0f);
//				var newY = Random.Range (area.y, 0f);
//				textMesh.transform.localPosition = new Vector3 (newX, newY, 0.0f);
//				textMesh.offsetZ = size;
				SetRandomPositionWithinBoundaries ();
				index++;
				lastTime = Time.time;
				renderer.material.SetColor ("_Color", color);
				alpha = renderer.material.GetColor ("_Color").a;
				textFadeSpeed = Time.deltaTime / timeForEachText;
		}

		public Vector2 minMaxX = new Vector2 (40.5f, 45.5f);
		public Vector2 minMaxY = new Vector2 (26f, 29f);

		void SetRandomPositionWithinBoundaries ()
		{
				bool useLeftSide = Random.Range (0, 2) == 0;
				var newX = 0f;
				var newY = 0f;
				if (useLeftSide) {
						textMesh.anchor = TextAnchor.UpperLeft;
						newX = Random.Range (-minMaxX.x, -minMaxX.y);
						newY = Random.Range (-minMaxY.x, minMaxY.y);
				} else {
						textMesh.anchor = TextAnchor.UpperRight;
						newX = Random.Range (minMaxX.x, minMaxX.y);
						newY = Random.Range (-minMaxY.x, minMaxY.y);
				}
				textMesh.transform.localPosition = new Vector3 (newX, newY, 0.0f);
		}
}
