using UnityEngine;
using System.Collections;

public class PhysicsHelper
{
		public static Vector3 CalculateGravity (Rigidbody A, Rigidbody B)
		{
				Vector3 dist = B.transform.position - A.transform.position; 
				float r = dist.magnitude;
				dist /= r;
				float G = 1f;// 6.67428x10-11; A.gravityScale;
				float m1 = A.mass;
				float m2 = B.mass;
				float F = (G * m1 * m2) / (r * r);
				return dist * F; // = force
		}

		public static Vector3 CalculateGravity (AstronomicalObject A, AstronomicalObject B)
		{
				return CalculateGravity (A.rigidbody, B.rigidbody);
		}

		public static Vector3 ApplyGravity (AstronomicalObject A, AstronomicalObject B)
		{
				var force = CalculateGravity (A, B);
				if (force.magnitude > Vector3.zero.magnitude) {
						A.rigidbody.AddForce (force);
						B.rigidbody.AddForce (-force);
				}
				return force;
		}
}
