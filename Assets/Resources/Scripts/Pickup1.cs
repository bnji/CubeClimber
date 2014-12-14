using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pickup1 : AbstractPickup<float>
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
//				Debug.Log (pickupValue + " coins given to player " + player.transform.name);
				// Increase the score
				player.IncreaseScore ((float)pickupValue);
				// Get Camera HUD and update the score
//				HUD hud = GameObject.FindObjectOfType<HUD> ();
//				if (hud != null)
//						hud.UpdateScore (pickupValue.ToString ());
				// Delete the health star pickup object
				if (destroyOnSend)
						Destroy (gameObject);
				PlayPickupSound ();
		}
}