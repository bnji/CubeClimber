using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pickup3 : AbstractPickup<float>
{
		public string playerTag;
	
		void OnTriggerEnter (Collider collider)
		{
				GameObject go = collider.gameObject;
				if (go == null)
						return;
		
//				if (go.tag == playerTag) {
//						var world = GameObject.FindObjectOfType<World> ();
//						world.SetCanRotate (pickupValue);
////						SendToPlayer (GameObject.FindObjectOfType<Player> ());
//				}
		}
	
		public override void SendToPlayer (Player player, bool destroyOnSend = true)
		{
				return;
		}
}