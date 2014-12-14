using UnityEngine;
using System.Collections;

public class World : MonoBehaviour
{

		public float speed;
		public Vector3 axis;

		private float _rotateToDegrees;

		private bool canRotate;



		public void SetCanRotate (float degrees)
		{
				_rotateToDegrees += degrees;
				canRotate = true;
		}

		void Start ()
		{
				canRotate = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (canRotate) {
						var angle = speed * Time.deltaTime;

						if (transform.rotation.eulerAngles.x < _rotateToDegrees) {
								transform.RotateAround (transform.position, axis, angle);
						} else {
								canRotate = false;
						}
				}
		}
}
