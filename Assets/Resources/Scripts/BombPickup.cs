using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombPickup : AbstractPickup<float>
{

		public GameObject explosionPrefab;
		private float lifeStarted;
		public float lifeSpan = 0f; // 0f = infinity
		public List<string> explodeByTags;
	
		void Start ()
		{
				lifeStarted = Time.time;
		}
	
		void Update ()
		{
				if (lifeSpan != 0 && Time.time - lifeStarted > lifeSpan) {
						Destroy (gameObject);
				}
		}
	
		void OnTriggerEnter (Collider collider)
		{
				GameObject go = collider.gameObject;
				if (go == null)
						return;

				if (go.tag == "Player") {
						/*Player player = go.GetComponent<Player> ();
						if (player == null)
								return;
						SendToPlayer (player);*/
						SendToPlayer (GameObject.FindObjectOfType<Player> ());
				} else {
						if (explodeByTags.Contains (go.tag)) {
								Destroy (go);
								Explode ();
						}
				}				
		}
	
		public override void SendToPlayer (Player player, bool destroyOnSend = true)
		{
				float newHealthValue = player.DecreaseHealth ((float)pickupValue);
				// Get Camera HUD and update the health
//				HUD hud = GameObject.FindObjectOfType<HUD> ();
//				if (hud != null)
//						hud.UpdateHealth (newHealthValue.ToString ());
				Explode ();
		}

		private float explodeAfterSeconds = 0f;

		public void ExplodeAfter (float seconds)
		{
				explodeAfterSeconds = seconds;
				StartCoroutine (ExplodeAfter ());
		}

		/// <summary>
		/// Explode this instance.
		/// TODO: StartAnimation scaleFrom and scaleTo should not be constants
		/// </summary>
		void Explode ()
		{
				// Delete the health pickup object
		
				GameObject explosionGameObject = (GameObject)Instantiate (explosionPrefab, transform.position, Quaternion.identity);
//				Explosion explisionAnimation = explosionGameObject.GetComponent<Explosion> ();
//				if (explisionAnimation != null) {
//						explisionAnimation.StartAnimation (0.1f, 0.5f);//transform.localScale.x);
//				}
				Destroy (gameObject);
				PlayPickupSound ();
		}

		IEnumerator ExplodeAfter ()
		{
				yield return new WaitForSeconds (explodeAfterSeconds);
				Explode ();
		}
}
