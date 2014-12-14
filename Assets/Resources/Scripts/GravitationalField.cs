using UnityEngine;
using System.Collections;

public class GravitationalField : MonoBehaviour
{
		public GameObject parent;

		void Awake ()
		{
				if (parent == null)
						parent = transform.parent.gameObject;//.GetComponent<PlanetScript> ();
		}

		void OnTriggerEnter (Collider collider)
		{
				PlanetScript otherPlanet = collider.gameObject.GetComponent<PlanetScript> ();
				if (otherPlanet != null)
						parent.SendMessage ("OnEnterAthmosphere", otherPlanet, SendMessageOptions.DontRequireReceiver);
		}

		void OnTriggerExit (Collider collider)
		{
				PlanetScript otherPlanet = collider.gameObject.GetComponent<PlanetScript> ();
				if (otherPlanet != null)
						parent.SendMessage ("OnExitAthmosphere", otherPlanet, SendMessageOptions.DontRequireReceiver);
		}
}
