using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{
		public float speed;
		public Vector3 axis;
	
		// Update is called once per frame
		void Update ()
		{
				transform.RotateAround (transform.position, axis, speed * Time.deltaTime);
		}
}
