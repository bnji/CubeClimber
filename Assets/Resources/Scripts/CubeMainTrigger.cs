using UnityEngine;
using System.Collections;

public class CubeMainTrigger : MonoBehaviour
{

		public Transform mainCube;
	
		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
		
		}
	
		void OnTriggerStay (Collider collider)
		{
				var mainCubeScript = mainCube.GetComponent<Cube> ();
				if (mainCubeScript != null) {
						mainCubeScript.CanWalkOnInvisibleCubes = true;
				}
		}
		void OnTriggerEnter (Collider collider)
		{
//				if (!CanShowInvisibleCube)
//						return;
//		
//		
				var player = collider.GetComponent<Player> ();
//				var mainCubeScript = mainCube.GetComponent<Cube> ();
				// Set current invisible cube the player is standing on
				if (player != null) {
						player.CurrentActiveCube = mainCube;
				}
		}
}
