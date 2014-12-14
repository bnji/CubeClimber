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

[System.Serializable]
public class PlayerData
{
		[SerializeField]
		private bool
				_isInvincible;
		public bool IsInvincible {
				get { return _isInvincible; }
				set { _isInvincible = value; }
		}
		[SerializeField]
		private float
				_id;
		public float ID {
				get { return _id; }
				set { _id = value; }
		}
		[SerializeField]
		private float
				_score;
		public float Score {
				get { return _score; }
				set {
						_score = value;
						PlayerPrefs.SetFloat (_id + "_score", value);
				}
		}
		[SerializeField]
		private float
				_health;
		public float Health {
				get { return _health; }
				set {
						_health = value; 
						PlayerPrefs.SetFloat (_id + "_health", value);
				}
		}
		[SerializeField]
		private float
				_maxHealth;
		public float MaxHealth {
				get { return _maxHealth; }
				set {
						_maxHealth = value; 
						PlayerPrefs.SetFloat (_id + "_maxHealth", value);
				}
		}
		[SerializeField]
		private float
				_lethalVelocity;
		public float LethalVelocity {
				get { return _lethalVelocity; }
				set {
						_lethalVelocity = value; 
						PlayerPrefs.SetFloat (_id + "_lethalVelocity", value);
				}
		}
		[SerializeField]
		private ControllerInfo
				_controllerInfo;
		public ControllerInfo ControllerInfo {
				get { return _controllerInfo; }
				set {
						_controllerInfo = value; 
						//PlayerPrefs.SetFloat (_id + "_controllerInfo", value);
				}
		}

		public void Load ()
		{
				_score = PlayerPrefs.GetFloat (_id + "_score");
				_health = PlayerPrefs.GetFloat (_id + "_health");
				_maxHealth = PlayerPrefs.GetFloat (_id + "_maxHealth");
				_lethalVelocity = PlayerPrefs.GetFloat (_id + "_lethalVelocity");
				//_controllerInfo = PlayerPrefs.GetFloat (_id + "_controllerInfo");
		}

		public void Reset ()
		{

		}
}