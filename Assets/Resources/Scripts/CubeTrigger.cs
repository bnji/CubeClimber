using UnityEngine;
using System.Collections;

public class CubeTrigger : MonoBehaviour
{

		public Transform mainCube;
		public Transform invisibleCube;

		public AudioClip enteredSound;

		private GameHandler gh;

		private float lastTimeEntered;

		// Use this for initialization
		void Start ()
		{
				lastTimeEntered = Time.time;
				gh = FindObjectOfType<GameHandler> ();
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
		void OnTriggerStay (Collider collider)
		{
				var mainCubeScript = mainCube.GetComponent<Cube> ();
				if (mainCubeScript != null) {
						mainCubeScript.canAddCube = true;
				}
//				gh.canAddCube = true;
				var player = collider.GetComponent<Player> ();
				// Set current invisible cube the player is standing on
				if (player != null) {
						player.currentMainCube = mainCube;
						player.currentInvisibleCube = invisibleCube;
				}
		}
	
		void OnTriggerEnter (Collider collider)
		{
				var mainCubeScript = mainCube.GetComponent<Cube> ();
				var invisibleCubeScript = invisibleCube.GetComponent<CubeInvisible> ();

				// If player can walk on invisible cubes (has landed (stayed) on the main cube)
				// then set last and current invisible cube
				if (mainCubeScript != null && mainCubeScript.canWalkOnInvisibleCubes) {

//						print (gh.canAddCube);

						// If player has never touched any invisible cubes, then set both to current invisible cube
						if (mainCubeScript.lastInvisibleCube == null && mainCubeScript.currentInvisibleCube == null) {
								mainCubeScript.lastInvisibleCube = invisibleCubeScript;
								mainCubeScript.currentInvisibleCube = invisibleCubeScript;
						} else {
								// else set last invisible cube to current one and current invisible cube to this one
//								mainCubeScript.lastInvisibleCube = mainCubeScript.currentInvisibleCube;
								mainCubeScript.currentInvisibleCube = invisibleCubeScript;

								if (mainCubeScript.currentInvisibleCube != null) {

										if ((Time.time - lastTimeEntered) * 1000f > 500f) {
												AudioSource.PlayClipAtPoint (enteredSound, transform.position);
												lastTimeEntered = Time.time;
										}
										mainCubeScript.currentInvisibleCube.GetComponent<MeshRenderer> ().enabled = true;
								}
						}
				}
		}

	
	
		void OnTriggerExit (Collider collider)
		{
//		var player = collider.GetComponent<Player> ();
//		if (player != null) {
//			player.l
//		}
				var mainCubeScript = mainCube.GetComponent<Cube> ();
				var invisibleCubeScript = invisibleCube.GetComponent<CubeInvisible> ();


//				gh.canAddCube = false;
		
				if (mainCubeScript != null) {// && mainCubeScript.canWalkOnInvisibleCubes) {
//						lastTimeEntered = Time.time;
			
						mainCubeScript.canAddCube = true;
						if (mainCubeScript.lastInvisibleCube != null) {// && ((Time.time - lastTimeEntered) * 1000f > 250f)) {

//				if (mainCubeScript.lastInvisibleCube != null) {
//					mainCubeScript.lastInvisibleCube.GetComponent<MeshRenderer> ().enabled = false;
//				}
								mainCubeScript.lastInvisibleCube = invisibleCubeScript;
//								if (invisibleCubeScript.name != mainCubeScript.currentInvisibleCube.name) {
								mainCubeScript.lastInvisibleCube.GetComponent<MeshRenderer> ().enabled = false;
//								mainCubeScript.lastInvisibleCube = null;
//								}
						}
				}
		}
}
