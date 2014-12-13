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
//				print ("Jump");
		}

		void OnLand ()
		{
				AudioSource.PlayClipAtPoint (landedSound, transform.position);
//				print ("Landed");
		}

		void OnFall ()
		{
				print ("Falling");
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.E) && ((Time.time - lastTimeHitKey) * 1000f > 250f)) {
						lastTimeHitKey = Time.time;
						var gh = FindObjectOfType<GameHandler> ();
						bool canAdd = true;
						if (currentInvisibleCube == null)
								return;
						var currentCubeScript = currentMainCube.GetComponent<Cube> ();
//						var currentInvisibleCubeScript = currentMainCube.GetComponent<Cube> ();
						Debug.Log (currentCubeScript);
						foreach (var cube in currentCubeScript.cubes) {// gh.cubes) {
								if (cube.transform.position == currentInvisibleCube.position) {
										canAdd = false;
										break;
								}
						}
//						print (canAdd + " " + gh.canAddCube);
						if (canAdd) {
								if (currentCubeScript.canAddCube) {//if (gh.canAddCube) {
										GameObject go = (GameObject)GameObject.Instantiate (cubePrefab, currentInvisibleCube.position, Quaternion.identity);
										go.transform.parent = currentMainCube;
										var cube = go.GetComponent<Cube> ();
										cube.SetSparkleState (false);
										currentCubeScript.AddCube (cube);
//										cube.AddCube (cube);
//										gh.AddCube (cube);
										AudioSource.PlayClipAtPoint (buildCubeSound, transform.position);
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

//		void OnCollisionEnter (Collision collision)
//		{
//				Debug.Log (collision.transform.tag);
//		
//		}
}
