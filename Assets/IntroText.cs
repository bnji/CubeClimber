using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IntroText : MonoBehaviour
{

		public TextMesh textMesh;
		public List<string> textList;
		public float timeForEachText = 2f;
		public Rect area; 

		private int index;
		private float lastTime;
		public Color color;
		private float textFadeSpeed = 0.1f;

		public float alpha;

		void Start ()
		{
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
				var size = textMesh.text.Length;
				var newX = Random.Range (area.x, 0f);
				var newY = Random.Range (area.y, 0f);
				textMesh.transform.localPosition = new Vector3 (newX, newY, 11.0f);
				textMesh.offsetZ = size;
				index++;
				lastTime = Time.time;
				renderer.material.SetColor ("_Color", color);
				alpha = renderer.material.GetColor ("_Color").a;
				textFadeSpeed = Time.deltaTime / timeForEachText;
		}
}
