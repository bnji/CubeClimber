using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{

		public List<Vector3> CubePositions;

		// Use this for initialization
		void Start ()
		{
				Screen.showCursor = false;
				PlayerPrefs.SetFloat ("BuildCubeAutoInterval", 2500f);
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
