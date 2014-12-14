using UnityEngine;
using System.Collections;

public class TriggerRotate : MonoBehaviour
{
		public string playerTag;
		public Transform objectToRotate;
		public float speed = 7f;
		public float degrees = 90f;
		public RotateAxis axis = RotateAxis.X;
		public bool useRandomAxis = false;
	
		void Awake ()
		{
				if (objectToRotate == null) {
						objectToRotate = transform.parent;
				}
		}
	
		void OnTriggerEnter (Collider collider)
		{
				GameObject go = collider.gameObject;
				if (go == null)
						return;
		
				if (go.tag == playerTag) {
						var rotateBase = objectToRotate.GetComponent<RotateBase> ();
						var _axis = axis;
						if (useRandomAxis) {
								var rn = Random.Range (0, 2);
								switch (rn) {
								case 0:
										_axis = RotateAxis.X;
										break;
								case 1:
										_axis = RotateAxis.Y;
										break;
								case 2:
										_axis = RotateAxis.Z;
										break;
								}
						} 
						Debug.Log (_axis);
						rotateBase.SetCanRotate (speed, degrees, _axis);
				}
		}
}
