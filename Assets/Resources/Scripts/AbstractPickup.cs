using UnityEngine;
using System.Collections;

public abstract class AbstractPickup<T> : AbstractPickupBase
{
		public AudioClip pickupSound;
		
		/// <summary>
		/// Pickup value
		/// </summary>
		public T pickupValue;

		private float createdOn;
		private float lifeTime = 0f;
	
		void Start ()
		{
				createdOn = Time.time;
		}

		void Update ()
		{
				if (lifeTime != 0f && Time.time - createdOn > lifeTime) {
						Destroy (gameObject);
				}
		}
		
		protected void PlayPickupSound ()
		{
				if (pickupSound != null) {
						StartCoroutine (PlaySound ());
				} else {
						Debug.Log ("Pickup Sound is NULL for " + this);
				}
		}
	
		IEnumerator PlaySound ()
		{
				AudioSource.PlayClipAtPoint (pickupSound, transform.position);
				yield return new WaitForSeconds (pickupSound.length);
		}

		public override void SetLifeTime (float value)
		{
				createdOn = Time.time;
				lifeTime = value;
		}
}

public abstract class AbstractPickupBase : MonoBehaviour
{
		public abstract void SendToPlayer (Player player, bool destroyOnSend = true);

		public abstract void SetLifeTime (float value);
}