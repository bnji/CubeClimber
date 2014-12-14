using UnityEngine;
using System.Collections;

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
}
