using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Helper
{
		private static float? lastTime = null;

		public static bool IsTimeUp (float timeLimit)
		{
				if (!lastTime.HasValue) {
						lastTime = Time.time;
						return false;
				} else {
						if ((Time.time - lastTime) * 1000f > timeLimit) {
								lastTime = Time.time;
								return true;
						}
				}
				return false;
		}
		
		public static void DestroyObjectAndChildren (Transform _transform)
		{
				if (_transform == null)
						return;
				var children = new List<GameObject> ();
				foreach (Transform child in _transform) {
						children.Add (child.gameObject);
				}
				children.ForEach (child => MonoBehaviour.Destroy (child));
				MonoBehaviour.Destroy (_transform.gameObject);
		}

}
