using UnityEngine;
using System.Collections;

public class CubeTrigger : MonoBehaviour
{

		public Transform mainCube;
		public Transform invisibleCube;
		public AudioClip enteredSound;

		private float timeSpentOnCube;
		private float lastTimeEntered;
		private GameHandler gh;
		private CubeInvisible invisibleCubeScript;
		private Player player;

		public bool CanShowInvisibleCube {
				get;
				set;
		}

		// Use this for initialization
		void Start ()
		{
				gh = FindObjectOfType<GameHandler> ();
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
				if (gh.canBuildCubeOnStay) {
//								if (Helper.IsTimeUp (PlayerPrefs.GetFloat ("BuildCubeAutoInterval")) && canBuild) {
						if (Helper.IsTimeUp (25f) && canBuild) {
								var player = GameObject.FindObjectOfType<Player> ();
								if (player != null && canBuild) {
										player.BuildCube ();
										canBuild = false;
								}
						}
				}
				var mainCubeScript = mainCube.GetComponent<Cube> ();
				if (mainCubeScript != null) {
						mainCubeScript.CanWalkOnInvisibleCubes = true;
				}
		}
	
		void OnTriggerEnter (Collider collider)
		{
				if (!CanShowInvisibleCube)
						return;

				player = collider.GetComponent<Player> ();
				var mainCubeScript = mainCube.GetComponent<Cube> ();
//				Debug.Log (mainCubeScript.transform.name);
				invisibleCubeScript = invisibleCube.GetComponent<CubeInvisible> ();
				// Set current invisible cube the player is standing on
				if (player != null) {

//						player.CurrentActiveCube = mainCube;
						player.CurrentMainCube = mainCube;
						player.CurrentInvisibleCube = invisibleCube;

						var diffY = collider.transform.position.y - transform.position.y;
//						Debug.Log (diffY);
						if (diffY > 0) {
								invisibleCubeScript.SetBoxTriggerState (false);
								invisibleCubeScript.SetVisible (true);
//								return;
						} else {
								invisibleCubeScript.SetBoxTriggerState (true);
								invisibleCubeScript.SetVisible (false);
						}

//						invisibleCubeScript.SetBoxTriggerState (false);


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
								mainCubeScript.LastInvisibleCube = mainCubeScript.CurrentInvisibleCube;
								mainCubeScript.CurrentInvisibleCube = invisibleCubeScript;

//								if (mainCubeScript.CurrentInvisibleCube != null) {
//										if ((Time.time - lastTimeEntered) * 1000f > 250f) {
//												AudioSource.PlayClipAtPoint (enteredSound, transform.position);
//												lastTimeEntered = Time.time;
//												canBuild = true;
//										}
//								}
						}
			
						if (player != null) {
								bool hasCubeAtGivePosition = false;
								var gh = GameObject.FindObjectOfType<GameHandler> ();
								foreach (var item in gh.CubePositions) {
//								foreach (var item in mainCubeScript.CubePositions) {
										if (item.Equals (invisibleCube.position)) {
												hasCubeAtGivePosition = true;
												Debug.Log ("there's already a cube here");
												break;
										}
								}
								if (!hasCubeAtGivePosition) {
//					if (mainCubeScript.CurrentInvisibleCube != null) {
										if ((Time.time - lastTimeEntered) * 1000f > 250f) {
												AudioSource.PlayClipAtPoint (enteredSound, transform.position);
												lastTimeEntered = Time.time;
												canBuild = true;
										}
//					}

										invisibleCubeScript.SetVisible (true);
										invisibleCubeScript.SetRandomColor ();//mainCubeScript.colors);
								}
						}
				}
		}

	
	
		void OnTriggerExit (Collider collider)
		{

//				var mainCubeScript = mainCube.GetComponent<Cube> ();
//				var invisibleCubeScript = invisibleCube.GetComponent<CubeInvisible> ();
//
//				if (mainCubeScript != null) {
//						if (mainCubeScript.LastInvisibleCube != null) {
//								mainCubeScript.LastInvisibleCube = invisibleCubeScript;
//								invisibleCubeScript.SetVisible (false);
//						}
//				}
				invisibleCubeScript.SetVisible (false);
				canBuild = false;
		
				if (player != null) {
						player.CurrentActiveCube = null;
				}
		}
}
