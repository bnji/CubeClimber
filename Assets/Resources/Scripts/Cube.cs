using UnityEngine;
using System.Collections;

public class Cube : MonoBehaviour
{

		public Transform CubeN;
		public Transform CubeNE;
		public Transform CubeE;
		public Transform CubeSE;
		public Transform CubeS;
		public Transform CubeSW;
		public Transform CubeW;
		public Transform CubeNW;

		public bool canWalkOnInvisibleCubes = false;
	
		public CubeInvisible lastInvisibleCube;
		public CubeInvisible currentInvisibleCube;
		public Transform sparkles;


		public void SetSparkleState (bool enabled)
		{
				sparkles.gameObject.SetActive (enabled);
		}

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
	
//		void OnCollisionEnter (Collision collision)
//		{
//				Debug.Log (collision.transform.tag);
//			
//		}

//		void OnTriggerStay (Collider collider)
//		{
//				Debug.Log (collider.transform.name);
//		}
}
