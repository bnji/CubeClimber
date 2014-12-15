using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class WeaponHolder : MonoBehaviour
{
	
		public ICarriable CurrentHeldItem { get; set; }
		public bool HasCollided { get; private set; }
	
		public void ChangeItem (ICarriable newItem)
		{
				if (CurrentHeldItem != null) {
						CurrentHeldItem.Release (); // release currently held item
				}
				CurrentHeldItem = newItem;
				HasCollided = false;
		}

		void OnTriggerEnter (Collider other)
		{
				HasCollided = true;
//				print ("weaponholder collided with: " + other.name);
		
		}
	
		void OnTriggerExit (Collider other)
		{
				HasCollided = false;
//		print ("weaponholder left: " + other.name);
		}
}
