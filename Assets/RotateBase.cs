using UnityEngine;
using System.Collections;

public class RotateBase : MonoBehaviour
{
		private float roateSpeed;
		private Vector3 rotateAroundAxis;
		private RotateAxis rotateAxis;
		private float rotateToDegrees;
        private bool isInitialRotation = false;
		private bool canRotate;
        private float rotateFromDegrees;
	
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
                        var angle2 = (angleRotated - rotateFromDegrees) / rotateToDegrees;
                        Debug.Log(angle2);
                        if (isInitialRotation)
                        {
                            transform.RotateAround(transform.position, rotateAroundAxis, angle);
                        }

                        if (angle2 == 0 || angle2 == 1)
                        {
                            canRotate = false;
                            Physics.gravity = new Vector3(0f, -9.82f, 0f);
                            isInitialRotation = false;
                        }
                        else
                        {
                            transform.RotateAround(transform.position, rotateAroundAxis, angle);
                        }
                        //Debug.Log(angleRotated + " / " + (rotateFromDegrees + rotateToDegrees) % 360);
                        //if (angleRotated < (rotateFromDegrees + rotateToDegrees) % 360) {
                        //        transform.RotateAround (transform.position, rotateAroundAxis, angle);
                        //} else {
                        //        canRotate = false;
                        //        Physics.gravity = new Vector3 (0f, -9.82f, 0f);
                        //}
				}
		}

		public void SetCanRotate (float speed, float degrees, RotateAxis axis)
		{
				if (!canRotate) {
						switch (axis) {
                            case RotateAxis.X:
                                rotateFromDegrees = transform.localRotation.eulerAngles.x;
								rotateAroundAxis = new Vector3 (1f, 0f, 0f);
								break;
                            case RotateAxis.Y:
                                rotateFromDegrees = transform.localRotation.eulerAngles.y;
								rotateAroundAxis = new Vector3 (0f, 1f, 0f);
								break;
                            case RotateAxis.Z:
                                rotateFromDegrees = transform.localRotation.eulerAngles.z;
								rotateAroundAxis = new Vector3 (0f, 0f, 1f);
								break;
						}
						rotateAxis = axis;
						roateSpeed = speed;
						rotateToDegrees += degrees;
						canRotate = true;
						Physics.gravity = new Vector3 (0f, 0f, 0f);
                        isInitialRotation = true;
				}
                Debug.Log("rotateFromDegrees: " + rotateFromDegrees);
		}
}

public enum RotateAxis
{
		X,
		Y,
		Z
}

