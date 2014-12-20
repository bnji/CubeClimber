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

		public List<Vector3> CubePositions;

		public void SetSparkleState (bool enabled)
		{
				sparkles.gameObject.SetActive (enabled);
		}

		private Color _startColor;

		public void SetColor (Color color)
		{
				//				renderer.material.shader = Shader.Find ("Specular");
				renderer.material.SetColor ("_Color", color);
		}

		public void ResetColor ()
		{
				//				renderer.material.shader = Shader.Find ("Specular");
				renderer.material.SetColor ("_Color", _startColor);
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
	
		public bool IsDestroyable { get; set; }

		public void Destroy ()
		{
				
				//RemoveCube ();
		}

	#endregion

		public void Detach ()
		{
				if (IsDestroyable) {
						var cubeTriggers = GetComponentsInChildren<CubeTrigger> ();
						foreach (var item in cubeTriggers) {
								item.CanShowInvisibleCube = false;
						}
						foreach (Transform child in transform) {
								GameObject.Destroy (child.gameObject);
						}
						var rb = GetComponent<Rigidbody> ();
						if (rb != null) {
								rb.isKinematic = false;
								rb.useGravity = true;
						}
				}
		}

		void Start ()
		{
				_startColor = renderer.material.GetColor ("_Color");
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