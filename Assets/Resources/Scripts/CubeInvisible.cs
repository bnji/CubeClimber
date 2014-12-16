﻿using UnityEngine;
using System.Collections;

public class CubeInvisible : CubeBase
{

		public Transform mainCube;

		private Cube mainCubeScript;

		private GameHandler gh;

		// Use this for initialization
		void Start ()
		{
				mainCubeScript = mainCube.GetComponent<Cube> ();
				gh = GameObject.FindObjectOfType<GameHandler> ();
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

		public void SetRandomColor ()//Color[] _colors)
		{
				renderer.material.color = gh.GetRandomColor ();
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
						var gh = GameObject.FindObjectOfType<GameHandler> ();

						var go = (GameObject)Instantiate (cubePrefab, transform.position, transform.rotation);
						var cube = go.GetComponent<Cube> ();
						(cube as IDestroyable).IsDestroyable = true;
						Debug.Log (renderer.material.color);
						cube.transform.renderer.material.color = gh.lastUsedColor;// renderer.material.color;
						cube.transform.parent = transform;
						cube.SetSparkleState (false);
						foreach (var item in cube.GetComponentsInChildren<TriggerRotate>()) {
								Destroy (item.gameObject);
						}
						cube.transform.name = "Cube #" + gh.CubePositions.Count;
						mainCubeScript.AddCube (cube);
						gh.CubePositions.Add (cube.transform.position);
						Debug.Log ("Added cube " + cube.transform.name + ". Cube count: " + gh.CubePositions.Count);
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
