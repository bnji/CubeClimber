using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{
	
		public bool useGlobalGravity = false;
		public List<Vector3> CubePositions;

		// Use this for initialization
		void Start ()
		{
				Screen.showCursor = false;
				PlayerPrefs.SetFloat ("BuildCubeAutoInterval", 2500f);
				_colors = new List<Vector3> (new Vector3[4] {
					new Vector3 (255f, 0f, 58f),
					new Vector3 (58f, 255f, 0f),
					new Vector3 (26f, 0f, 225f),
					new Vector3 (255f, 219f, 0f)
				});
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
		private List<Vector3> _colors;
		int c1 = 0;
		public Color lastUsedColor = new Color (0f, 0f, 0f);

		public Color GetRandomColor ()
		{
				var index = c1++ % _colors.Count;// Random.Range (0, _colors.Length - 1);
				Debug.Log (index);
				Color color = new Color (_colors [index].x, _colors [index].y, _colors [index].z, 0f);
//				Debug.Log (color);
				//				if (lastUsedColor.Equals (color)) {
				//						SetRandomColor (colors);
				//				} else {
				lastUsedColor = color;
				return color;
				//				}
		}
}
