using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerBox : MonoBehaviour
{
		public List<Transform> validColliders;

		void Start ()
		{
				if (validColliders == null || validColliders.Count == 0) {
						validColliders = new List<Transform> ();
						validColliders.Add (GameObject.FindGameObjectWithTag ("Player").transform);
				}
		}

		void OnTriggerEnter (Collider collider)
		{
				if (validColliders.Contains (collider.transform)) {
						gameObject.SendMessageUpwards ("OnCollidedWithTriggerBox", SendMessageOptions.DontRequireReceiver);
				}
		}
}
