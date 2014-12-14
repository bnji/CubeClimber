using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class CubeBase : MonoBehaviour
{

}

public class Cube : CubeBase, IDestroyable
{

		public Color[] colors;

		public Transform CubeN;
		public Transform CubeNE;
		public Transform CubeE;
		public Transform CubeSE;
		public Transform CubeS;
		public Transform CubeSW;
		public Transform CubeW;
		public Transform CubeNW;

		public bool CanWalkOnInvisibleCubes { get; set; }
		public CubeInvisible LastInvisibleCube { get; set; }
		public CubeInvisible CurrentInvisibleCube { get; set; }
		public Transform sparkles;


		public void SetSparkleState (bool enabled)
		{
				sparkles.gameObject.SetActive (enabled);
		}

//		public void SetVisible (bool visible)
//		{
//				transform.GetComponent<MeshRenderer> ().enabled = visible;
//		}
	
		public List<Cube> cubes;
	
		public void AddCube (Cube cube)
		{
				cubes.Add (cube);
		}
	
		private void RemoveCube (Cube cube)
		{
//				foreach (var item in cube.cubes) {
//						item.RemoveCube ();
//				}
				cubes.Remove (cube);
		}

		public void RemoveCube ()
		{
				RemoveCube (this);
				Destroy (gameObject);
		}

	#region IDestroyable implementation

		public void Destroy ()
		{
				//RemoveCube ();
		}

	#endregion

		void Start ()
		{

		}

		// Update is called once per frame
		void Update ()
		{
		}

		public bool CanAdd ()
		{
				bool canAdd = true;
				foreach (var cube in cubes) {
//						Debug.Log (cube.name);
						if (cube != null && cube.transform.position == transform.position) {
								canAdd = false;
								break;
						}
				}
				return canAdd;
		}

		public Cube ()
		{
				cubes = new List<Cube> ();
				CanWalkOnInvisibleCubes = false;
		}

}