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
				try {
						var index = Random.Range (0, colors.Length - 1);
						Color color = colors [index];
						if (lastUsedColor.Equals (color)) {
								SetRandomColor (colors);
						} else {
								lastUsedColor = color;
								renderer.material.color = color;
						}
				} catch (UnityException ex) {
						Debug.Log (ex.Message);
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
						foreach (var item in cube.GetComponentsInChildren<TriggerRotate>()) {
								Destroy (item.gameObject);
						}
						mainCubeScript.AddCube (cube);
						var gh = GameObject.FindObjectOfType<GameHandler> ();
						gh.CubePositions.Add (cube.transform.position);

			
			
						//hasVisibleCube = true;
						//						foreach (var item in cube.GetComponentsInChildren<CubeInvisible>()) {
						//								if (gh.CubePositions.Contains (item.transform.position)) {
						//										Debug.Log (Vector3.Distance (item.transform.localPosition, cube.transform.localPosition));
						//								}
						//						}
			
			
			
						//						foreach (var item in cube.GetComponentsInChildren<CubeBase>()) {
						//								Debug.Log (Vector3.Distance (item.transform.localPosition, mainCube.localPosition) + " " + item.transform.parent.name + "." + item.name + " - " + mainCube.parent.name + "." + mainCube.name);
						//								foreach (var item2 in GetComponentsInChildren<CubeBase>()) {
						//										var distance = Vector3.Distance (item.transform.localPosition, item2.transform.localPosition);
						//										if (distance == 0f && item != item2)  
						//												Debug.Log (distance + " " + item.transform.parent.name + "." + item.name + " - " + item2.transform.parent.name + "." + item2.name);
						////										if (item.hasVisibleCube) {// && Vector3.Distance (item.transform.localPosition, item2.transform.localPosition) == 0f) {
						////												Debug.Log (item.hasVisibleCube + " " + item2.hasVisibleCube + " " + Vector3.Distance (item.transform.localPosition, item2.transform.localPosition) + " " + item.transform.parent.name + "." + item.name + " - " + item2.transform.parent.name + "." + item2.name);
						////										}
						////					    {
						////												item.GetComponentInChildren<CubeTrigger> ().isUsable = false;
						////										}
						////										if (item.mainCube.position == item2.transform.position) {
						////												item.GetComponentInChildren<CubeTrigger> ().isUsable = false;
						////										}
						//								}
						//						}
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
				if (collider.transform.tag == "Player") {
			
						var springJoint = transform.GetComponentInChildren<SpringJoint> ();
//						Debug.Log (springJoint);
						if (springJoint != null) {
								springJoint.connectedBody = collider.rigidbody;
						}
				}
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
