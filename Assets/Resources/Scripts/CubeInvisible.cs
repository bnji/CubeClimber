using UnityEngine;
using System.Collections;

public class CubeInvisible : MonoBehaviour
{

		public Transform mainCube;

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (mainCube != null) {
						var mainCubeCtl = mainCube.GetComponent<Cube> ();
						if (mainCubeCtl.canWalkOnInvisibleCubes) {
								var boxCollider = GetComponent<BoxCollider> ();
								boxCollider.isTrigger = false;
						}
				}
		}

//		void OnTriggerEnter (Collider collider)
//		{
//				Debug.Log (this.name);
//		}

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
