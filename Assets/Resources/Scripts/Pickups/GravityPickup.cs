using UnityEngine;
using System.Collections;

public class GravityPickup : AbstractPickup<float>
{
		public string playerTag;
	
		void OnTriggerEnter (Collider collider)
		{
				GameObject go = collider.gameObject;
				if (go == null)
						return;
		
				if (go.tag == playerTag) {
						SendToPlayer (GameObject.FindObjectOfType<Player> ());
				}
		}
	
		public override void SendToPlayer (Player player, bool destroyOnSend = true)
		{
				player.SetGravity (pickupValue);
				// Delete the health star pickup object
				if (destroyOnSend) {
						Destroy (gameObject);
				}
				PlayPickupSound ();
		}
}