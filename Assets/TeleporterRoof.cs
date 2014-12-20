using UnityEngine;
using System.Collections;

public class TeleporterRoof : MonoBehaviour
{
		void OnTriggerEnter (Collider collider)
		{
				Debug.Log (collider.transform.name);
				var player = collider.gameObject.GetComponent<Player> ();
				if (player != null) {
				}
		}
}