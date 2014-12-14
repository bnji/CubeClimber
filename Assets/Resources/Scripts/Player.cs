using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
		public Object cubePrefab;
		public Transform currentMainCube;
		public Transform currentInvisibleCube;

		public AudioClip jumpSound;
		public AudioClip landedSound;
		public AudioClip buildCubeSound;

		// Use this for initialization
		void Start ()
		{
				lastTimeHitKey = Time.time;
		}

		float lastTimeHitKey;

		void OnJump ()
		{
				AudioSource.PlayClipAtPoint (jumpSound, transform.position);


				if (currentMainCube != null) {
						var currentCubeScript = currentMainCube.GetComponent<Cube> ();
						var cubeTriggers = currentCubeScript.GetComponentsInChildren<CubeTrigger> ();
						foreach (var item in cubeTriggers) {
								item.CanShowInvisibleCube = false;
						}
						foreach (Transform child in currentMainCube.transform) {
								GameObject.Destroy (child.gameObject);
						}
						var rb = currentMainCube.GetComponent<Rigidbody> ();
						rb.isKinematic = false;
						rb.useGravity = true;
//						currentCubeScript.Destroy ();
				}
		}

		void OnLand ()
		{
				AudioSource.PlayClipAtPoint (landedSound, transform.position);
		}

		void OnFall ()
		{
//				print ("Falling");
		}

		public void BuildCube ()
		{
				if (currentInvisibleCube == null)
						return;

				var currentInvisibleCubeScript = currentInvisibleCube.GetComponent<CubeInvisible> ();
				if (currentInvisibleCubeScript.BuildCube (cubePrefab)) {
						AudioSource.PlayClipAtPoint (buildCubeSound, transform.position);
						Debug.Log (GameObject.FindObjectOfType<GameHandler> ().CubePositions.Count);
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.E) && ((Time.time - lastTimeHitKey) * 1000f > 250f)) {
						if (currentInvisibleCube != null) {
								var currentInvisibleCubeScript = currentInvisibleCube.GetComponent<CubeInvisible> ();
								if (currentInvisibleCubeScript.BuildCube (cubePrefab)) {
										AudioSource.PlayClipAtPoint (buildCubeSound, transform.position);
										Debug.Log (GameObject.FindObjectOfType<GameHandler> ().CubePositions.Count);
										lastTimeHitKey = Time.time;
								}
						}
				}

//				Vector3 fwd = transform.TransformDirection (Vector3.forward);
//				RaycastHit rayCastHit;
//		
//				Ray ray = Camera.main.ViewportPointToRay (Input.mousePosition);
//				if (Physics.Raycast (transform.position, fwd, out rayCastHit, 10000)) {
//						Debug.DrawLine (ray.origin, rayCastHit.point);
//						var cubeHit = rayCastHit.transform.GetComponent<Cube> ();
//						if (cubeHit != null) {
//								print (cubeHit.transform.name);
//						}
////						print (rayCastHit.transform.name);
//				}
		}
}
