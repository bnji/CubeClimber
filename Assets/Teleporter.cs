using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
		public Transform roof;
		public float roofSpeed = 0.25f;
		public float scaleMult = 2f;
		Vector3 originalScale;
		bool isPlayerInside = false;
		Player player;

		// Use this for initialization
		void Start ()
		{
				originalScale = transform.localScale;
		}

	
		// Update is called once per frame
		void Update ()
		{
				if (isPlayerInside) {
						var dest = new Vector3 (roof.position.x, roof.position.y - roofSpeed * Time.deltaTime, roof.position.z);
						roof.position = dest;
						//			               roof.position = Destroy;
						//						Debug.Log (roof.position.y + " / " + player.transform.position.y);
						//						if (roof.position.y <= player.transform.position.y + player.transform.localScale.y) {
						//								Debug.Log ("player died");
						//						}
				}
		}
	
		void OnTriggerEnter (Collider collider)
		{
				player = collider.gameObject.GetComponent<Player> ();
				if (player != null) {
						isPlayerInside = true;
						var newY = originalScale.y * scaleMult;
						transform.localScale = new Vector3 (originalScale.x * scaleMult, originalScale.y * scaleMult, originalScale.z * scaleMult);
						transform.position = new Vector3 (transform.position.x, transform.position.y + (newY / originalScale.y) + 1f, transform.position.z);
				}
		}
	
		void OnTriggerExit (Collider collider)
		{
				isPlayerInside = false;
				transform.localScale = new Vector3 (originalScale.x, originalScale.y, originalScale.z);
		}
}
