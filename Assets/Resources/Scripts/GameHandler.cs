using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameHandler : MonoBehaviour
{

		public List<Cube> cubes;

		public bool canAddCube = false;

		public void AddCube (Cube cube)
		{
				cubes.Add (cube);
		}

		public void RemoveCube (Cube cube)
		{
				cubes.Remove (cube);
		}

		// Use this for initialization
		void Start ()
		{
				Screen.showCursor = false;

				cubes = new List<Cube> (GameObject.FindObjectsOfType<Cube> ());
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
