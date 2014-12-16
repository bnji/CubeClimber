using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerEnter (Collider collider)
		{
				var go = collider.gameObject;
				var cubeRigidBody = go.GetComponent<Rigidbody> ();
				if (cubeRigidBody != null) {
						cubeRigidBody.isKinematic = false;
						cubeRigidBody.useGravity = true;
				}
		}
}
