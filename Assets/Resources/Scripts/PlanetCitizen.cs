using UnityEngine;
using System.Collections;

public abstract class PlanetCitizen : MonoBehaviour
{
		public SpaceCharacterController controller;
		public PlayerData data;
		protected PlanetScript playerPlanetScript;
		protected float gravityScaleOnStart;
		private float initialDrag;
		private float initialMass;

		protected PlanetScript GetCurrentPlanetScript ()
		{
				if (controller.currentPlanet != null) {
						return controller.currentPlanet.GetComponent<PlanetScript> ();
				} 
				return null;
		}

		protected void ResetDrag ()
		{
				rigidbody.drag = initialDrag;
		}

		protected void ResetMass ()
		{
				rigidbody.mass = initialMass;
		}


		protected bool isPaused;
		public void OnPauseGame ()
		{
				isPaused = true;

		}
	
		public void OnResumeGame (bool hideCursor)
		{
				isPaused = false;
		}

		protected void Initialize ()
		{
				initialDrag = rigidbody.drag;
				initialMass = rigidbody.mass;
				transform.localScale = newScale;
				newScaleConstant = newScale.x;
				playerPlanetScript = GetComponent<PlanetScript> ();
//				gravityScaleOnStart = playerPlanetScript.gravityScale;
		}

		protected Vector3 newScale = new Vector3 (0.3f, 0.3f, 0.3f);
		protected float newScaleConstant;
		protected void Inflate ()
		{
				float newScaleInc = (controller.ControllerInfo.JumpPowerIncrement / controller.ControllerInfo.MaxJumpPower) * newScaleConstant;
				newScale = new Vector3 (newScale.x + newScaleInc, newScale.y + newScaleInc, newScale.z + newScaleInc);
				transform.localScale = new Vector3 (newScale.x, newScale.y, newScale.z);
		}
}