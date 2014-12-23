using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugText : MonoBehaviour
{
		private TextMesh textMesh;
		private float alpha;

		public void SetText (string text)
		{
				textMesh.text = text;
		}
	
		void Start ()
		{
				textMesh = GetComponent<TextMesh> ();
		}
}
