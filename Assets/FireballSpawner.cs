using UnityEngine;
using System.Collections;

public class FireballSpawner : MonoBehaviour
{
		public Object fireballPrefab;
		public float minInterval;
		public float maxInterval;

		public Vector3 minimum;
		public Vector3 maximum;

		private float interval;

		private float GetNewInterval ()
		{
				return Random.Range (minInterval, maxInterval);
		}

		// Use this for initialization
		void Start ()
		{
				interval = GetNewInterval ();
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Helper.IsTimeUp (interval)) {
						var go = (GameObject)Instantiate (fireballPrefab, new Vector3 (Random.Range (minimum.x, maximum.x), Random.Range (minimum.y, maximum.y), Random.Range (minimum.z, maximum.z)), Quaternion.identity);
						interval = GetNewInterval ();
				}
		}
}
