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
}
