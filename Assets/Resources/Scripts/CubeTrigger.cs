using UnityEngine;
using System.Collections;

public class CubeTrigger : MonoBehaviour
{

		public Transform mainCube;
		public Transform invisibleCube;
		public AudioClip enteredSound;

		private float timeSpentOnCube;
		private float lastTimeEntered;

		public bool CanShowInvisibleCube {
				get;
				set;
		}

		// Use this for initialization
		void Start ()
		{
				CanShowInvisibleCube = true;
				lastTimeEntered = Time.time;
		}
	
		// Update is called once per frame
		void Update ()
		{
		}

		public bool canBuild = false;
	
		void OnTriggerStay (Collider collider)
		{
				if (((Time.time - lastTimeEntered) * 1000f > PlayerPrefs.GetFloat ("BuildCubeAutoInterval")) && canBuild) {
						var player = GameObject.FindObjectOfType<Player> ();
						if (player != null && canBuild) {
								player.BuildCube ();
								canBuild = false;
						}
				}
		}

		CubeInvisible invisibleCubeScript;
	
		void OnTriggerEnter (Collider collider)
		{
				if (!CanShowInvisibleCube)
						return;


				var player = collider.GetComponent<Player> ();
				var mainCubeScript = mainCube.GetComponent<Cube> ();
				invisibleCubeScript = invisibleCube.GetComponent<CubeInvisible> ();
				// Set current invisible cube the player is standing on
				if (player != null) {
						player.currentMainCube = mainCube;
						player.currentInvisibleCube = invisibleCube;
				}

				// If player can walk on invisible cubes (has landed (stayed) on the main cube)
				// then set last and current invisible cube
				if (mainCubeScript != null && mainCubeScript.CanWalkOnInvisibleCubes) {
						// If player has never touched any invisible cubes, then set both to current invisible cube
						if (mainCubeScript.LastInvisibleCube == null && mainCubeScript.CurrentInvisibleCube == null) {
								mainCubeScript.LastInvisibleCube = invisibleCubeScript;
								mainCubeScript.CurrentInvisibleCube = invisibleCubeScript;
						} else {
								// else set last invisible cube to current one and current invisible cube to this one
//								mainCubeScript.lastInvisibleCube = mainCubeScript.currentInvisibleCube;
								mainCubeScript.CurrentInvisibleCube = invisibleCubeScript;
								if (mainCubeScript.CurrentInvisibleCube != null) {
										if ((Time.time - lastTimeEntered) * 1000f > 250f) {
												AudioSource.PlayClipAtPoint (enteredSound, transform.position);
												lastTimeEntered = Time.time;
												canBuild = true;
										}
								}
						}
			
						if (player == null)
								return;
			
						invisibleCubeScript.SetVisible (true);
						invisibleCubeScript.SetRandomColor (mainCubeScript.colors);
				}
		}

	
	
		void OnTriggerExit (Collider collider)
		{
				var mainCubeScript = mainCube.GetComponent<Cube> ();
				var invisibleCubeScript = invisibleCube.GetComponent<CubeInvisible> ();

				if (mainCubeScript != null) {
						if (mainCubeScript.LastInvisibleCube != null) {
								mainCubeScript.LastInvisibleCube = invisibleCubeScript;
								invisibleCubeScript.SetVisible (false);
						}
				}
				canBuild = false;
		}
}
