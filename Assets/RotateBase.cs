using UnityEngine;
using System.Collections;

public class RotateBase : MonoBehaviour
{
		private float roateSpeed;
		private Vector3 rotateAroundAxis;
		private RotateAxis rotateAxis;
		private float rotateToDegrees;
		private bool canRotate;
	
		void Start ()
		{
				canRotate = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (canRotate) {
						var angle = roateSpeed * Time.deltaTime;
//						Debug.Log (transform.rotation.eulerAngles.x + " / " + rotateToDegrees);
						var angleRotated = 0f;
						switch (rotateAxis) {
						case RotateAxis.X: 
								angleRotated = transform.rotation.eulerAngles.x;
								break;
						case RotateAxis.Y: 
								angleRotated = transform.rotation.eulerAngles.y;
								break;
						case RotateAxis.Z: 
								angleRotated = transform.rotation.eulerAngles.z;
								break;
						}
						if (angleRotated < rotateToDegrees) {
								transform.RotateAround (transform.position, rotateAroundAxis, angle);
						} else {
								canRotate = false;
						}
				}
		}

		public void SetCanRotate (float speed, float degrees, RotateAxis axis)
		{
				if (!canRotate) {
						switch (axis) {
						case RotateAxis.X: 
								rotateAroundAxis = new Vector3 (1f, 0f, 0f);
								break;
						case RotateAxis.Y: 
								rotateAroundAxis = new Vector3 (0f, 1f, 0f);
								break;
						case RotateAxis.Z: 
								rotateAroundAxis = new Vector3 (0f, 0f, 1f);
								break;
						}
						rotateAxis = axis;
						roateSpeed = speed;
						rotateToDegrees += degrees;
						canRotate = true;
				}
		}
}

public enum RotateAxis
{
		X,
		Y,
		Z
}

