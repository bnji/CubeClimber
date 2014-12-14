using UnityEngine;
using System.Collections;

public class DeathBox : MonoBehaviour
{

		void OnTriggerEnter (Collider collider)
		{
				var destoyable = collider as IDestroyable;
				if (destoyable != null) {
						destoyable.Destroy ();
				}
		}
}
