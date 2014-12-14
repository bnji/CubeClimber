using UnityEngine;
using System.Collections;

public class CubeInvisible : CubeBase
{

		public Transform mainCube;

		private Cube mainCubeScript;
		private Color lastUsedColor = new Color (0f, 0f, 0f);

		// Use this for initialization
		void Start ()
		{
				mainCubeScript = mainCube.GetComponent<Cube> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (mainCubeScript != null) {
						if (mainCubeScript.CanWalkOnInvisibleCubes) {
								var boxCollider = GetComponent<BoxCollider> ();
								boxCollider.isTrigger = false;
						}
				}
		}
	
		public void SetVisible (bool visible)
		{
				transform.GetComponent<MeshRenderer> ().enabled = visible;
		}

		public void SetRandomColor (Color[] colors)
		{
				Color color = colors [Random.Range (0, colors.Length)];
				if (lastUsedColor.Equals (color)) {
						SetRandomColor (colors);
				} else {
						lastUsedColor = color;
						renderer.material.color = color;
				}
		}
	
		public bool CanAdd ()
		{
				bool canAdd = true;
				foreach (var cube in mainCubeScript.cubes) {
						if (cube != null && cube.transform.position == transform.position) {
								canAdd = false;
								break;
						}
				}
				return canAdd;
		}

		public bool BuildCube (Object cubePrefab)
		{
				if (CanAdd ()) {
						var go = (GameObject)Instantiate (cubePrefab, transform.position, transform.rotation);
						var cube = go.GetComponent<Cube> ();
						cube.renderer.material.color = renderer.material.color;
						cube.transform.parent = transform;
						cube.SetSparkleState (false);
						mainCubeScript.AddCube (cube);
						var gh = GameObject.FindObjectOfType<GameHandler> ();
						gh.CubePositions.Add (cube.transform.position);
						return true;
				}
				return false;
		}

		public bool canBuild = true;

//		void OnTriggerStay (Collider collider)
//		{
//				Debug.Log (collider.name);
//		}
//
		void OnTriggerExit (Collider collider)
		{
				canBuild = false;
//				Debug.Log (collider.name);
		}

		void OnTriggerEnter (Collider collider)
		{
				canBuild = true;
//				Debug.Log (this.name);
		}

//		void OnCollisionEnter (Collision collision)
//		{
//				if (collision.transform.tag == "Player") {
//						Debug.Log ("hello");
//				}
////				foreach (ContactPoint contact in collision.contacts) {
////						Debug.DrawRay (contact.point, contact.normal, Color.white);
////				}
////				if (collision.relativeVelocity.magnitude > 2)
////						audio.Play ();
//		
//		}
}
