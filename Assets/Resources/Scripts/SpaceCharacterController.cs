using UnityEngine;
using System.Collections;

public enum TurnDir
{
		LEFT,
		RIGHT
}

public class SpaceCharacterController : MonoBehaviour
{
	
		protected TurnDir turnDir;
		public float jumpInterval = 0.5f;

		public ForceMode forceMode = ForceMode.Force;
		[Range(0.01f, 5f)]
		public float
				maxTurnSpeed = 0.1f;
		[Range(0.01f, 5f)]
		public float
				turnSpeedDamingFactor = 0.5f;

		[Range(0.01f, 5f)]
		public float
				initialTurningVelocity = 0.5f;


	
		protected Vector3 CalculateForce (float dirSpeed)
		{
				return transform.right * turnSpeedDamingFactor * dirSpeed * Time.deltaTime;
		}
	
		public float CurrentVelocity {
				get {
						return rigidbody.velocity.sqrMagnitude;
				}
		}


		public bool canControlPlayer = true;
		public bool HasBeenTooLongInSpace {
				get;
				set;
		}
		
		// Space char controller
		public float JumpPower {
				get;
				set;
		}
		protected bool jumpPowerDirection = true;
		protected float currentRotationSpeed;
		public float lastTimeJumped;
		protected float lastTimeLanded;
		public bool isInAir = true;
		public GameObject currentPlanet;
		public GameObject lastPlanet;


		protected ControllerInfo _controllerInfo;
		public ControllerInfo ControllerInfo {
				get { return _controllerInfo; }
		}

		protected Quaternion rotationBeforeJump;
		float lastTimeEnteredPlanet;

		protected bool isPaused;
		public void OnPauseGame ()
		{
				isPaused = true;
		}
	
		public void OnResumeGame (bool hideCursor)
		{
				isPaused = false;
		}
	
		void Awake ()
		{
				
		}

		void Start ()
		{
				JumpPower = 0f;
//				currentRotationSpeed = _controllerInfo.RotateSpeed;
				lastTimeJumped = Time.time;
				lastTimeLanded = Time.time;
				lastTimeEnteredPlanet = Time.time;

		}

		protected virtual void FixedUpdate ()
		{
				if (isPaused)
						return;
				if (!canControlPlayer)
						return;

				if (currentPlanet == null && isInAir) {
						if (ControllerInfo != null) {
								gameObject.SendMessage ("OnFloatingInSpace", ControllerInfo, SendMessageOptions.DontRequireReceiver);
						}			
						
				} else {
						// The player is on a planet
						// Reset the player rotation
						SetRotation ();
				}
		}
	
	
		void OnCollisionEnter (Collision collision)
		{
		
				GameObject go = collision.gameObject;
				AttachToPlanet (go);
		}
	
		void OnCollisionExit (Collision collision)
		{
				GameObject go = collision.gameObject;
				//DetachFromPlanet (go);
		}
	
		void OnCollisionStay (Collision collision)
		{
		
		}

		protected void Jump ()
		{
				// Jump from a planet into space and hopefully land on another planet
				// Only jump if enough power (hold space long enough, but not too long)
				// Don't jump if not on a planet
				gameObject.SendMessage ("OnJumpButtonReleased", SendMessageOptions.DontRequireReceiver);
				if (ControllerInfo != null && JumpPower >= ControllerInfo.MinJumpPower && Time.time - lastTimeJumped > ControllerInfo.JumpInterval && currentPlanet != null) {
						Jump (JumpPower);
				}
				JumpPower = 0.0f;
		}
	
		protected void Jump (float jumpPower)
		{
				gameObject.SendMessage ("OnJump", jumpPower, SendMessageOptions.DontRequireReceiver);
				//Debug.Log (jumpPower);
				
//				Vector3 from = new Vector3(transform.position.x, transform.position.y, tran);
//				Vector3 to = new Vector3 (currentPlanet.transform.position.x, currentPlanet.transform.position.y);
				JumpDirection = transform.position - currentPlanet.transform.position;
				rigidbody.AddForce (JumpDirection * jumpPower * Time.deltaTime);
				Detach ();
		}

		public void LeavePlanet (GameObject planet)
		{
				currentPlanet = planet;
				Jump (ControllerInfo.MinJumpPower);
		}

		private void Detach ()
		{
				if (rotationBeforeJump == Quaternion.identity) {
						rotationBeforeJump = transform.localRotation;
				}
				// Disconnect the spring joint
//				DisableSpringJoint ();
				
		
				// Remember which our last planet was before setting current planet to null
				lastPlanet = currentPlanet;
				if (lastPlanet != null) {
						PlanetScript ps = lastPlanet.GetComponent<PlanetScript> ();
						if (ps != null) {
								// Send message to the planet that the player has left it
								ps.SendMessage ("OnObjectExit", gameObject, SendMessageOptions.DontRequireReceiver);
						}
				}
				currentPlanet = null;
				lastTimeJumped = Time.time;
				isInAir = true;
		}

		public Vector3 JumpDirection { get; private set; }
	
		void SetRotation ()
		{
				if (currentPlanet != null) {
						
						Vector3 dir = currentPlanet.transform.position - transform.position;
						Quaternion QuaternionRotation = Quaternion.FromToRotation (transform.up, (transform.position - currentPlanet.transform.position).normalized);
						transform.rotation = QuaternionRotation * transform.rotation;
						//Debug.DrawRay (from, dir, Color.green);
				}
		}
	
		void DisableSpringJoint ()
		{
				var springJoint2D = GetComponent<SpringJoint2D> (); //GetComponent<DistanceJoint2D> ();
				if (springJoint2D != null) {
						springJoint2D.enabled = false;
				}
		}

		void AttachToPlanet (GameObject otherGameObject)
		{
				PlanetScript planet = otherGameObject.GetComponent<PlanetScript> ();
				if (planet == null)
						return;
				// Don't process if can't land on planet (for example if it's a health pickup)
				if (!planet.canLandOnPlanet)
						return;

				//Debug.Log ("Player landed on planet!!");
				// Send message to the planet that the player has landed on it
				//Send message to the planet
				planet.gameObject.SendMessage ("OnObjectEnter", gameObject, SendMessageOptions.DontRequireReceiver);

				// Send a message to the player 
				gameObject.SendMessage ("OnObjectEnter2", otherGameObject, SendMessageOptions.DontRequireReceiver);


				// FIX THIS! Currently this is a workaround when entering planets which rotate, that makes the player attached to the planet but in a long distance from it.
				//if (go.GetComponent<PlanetScript> ().isRotating)
				//		go.GetComponent<PlanetScript> ().isRotating = false;
		
				//Debug.Log (Time.time - lastTimeJumped);
				// only attach a planet if current planet is not set and landed more than 2 seconds ago
				if (currentPlanet == null) {// && Time.time - lastTimeJumped > 2f) {
						
						gameObject.SendMessage ("OnObjectAttached", planet, SendMessageOptions.DontRequireReceiver);

						//canControlPlayer = true;
			
						lastTimeEnteredPlanet = Time.time;
						
						//Debug.Log ("entered a planet");
						// Set the new planet the player landed on to be the current planet
						currentPlanet = otherGameObject;

						//Debug.Log (currentPlanet.name + " isIndestructible? " + currentPlanet.GetComponent<PlanetScript> ().isIndestructible);
						// Make sure the current planet we landed on won't get destroyed
						currentPlanet.GetComponent<PlanetScript> ().isIndestructible = true;
						//Debug.Log (currentPlanet.name + " isIndestructible? " + currentPlanet.GetComponent<PlanetScript> ().isIndestructible);
						isInAir = false;
						
						//Debug.Log ("player landed on a planet called '" + collision.gameObject.name + "'");
			
						lastTimeLanded = Time.time;
			
						//if (animator != null)
						//animator.enabled = true;

						//ControllerInfo.GravityScale = 0f;
				}
		}
	
		void DetachFromPlanet (GameObject go)
		{
				// Don't process if can't land on planet (for example if it's a health pickup)
				PlanetScript planetScript = go.GetComponent<PlanetScript> ();
				//if (planetScript != null)// && !planetScript.canLandOnPlanet)
				//		return;
		
				if (planetScript == null)
						return;
		
				// Send message to the planet that the player has left it
				planetScript.SendMessage ("OnObjectExit", gameObject, SendMessageOptions.DontRequireReceiver);
		}


		public void Setup (ControllerInfo controllerInfo)
		{
				_controllerInfo = controllerInfo;
		}
}

[System.Serializable]
public class ControllerInfo
{
		[SerializeField]
		private float
				_rotateSpeedWhileInAir;
		public float RotateSpeedWhileInAir {
				get { return _rotateSpeedWhileInAir; }
				set { _rotateSpeedWhileInAir = value; }
		}
		[SerializeField]
		private float
				_rotateSpeed;
		public float RotateSpeed {
				get { return _rotateSpeed; }
				set { _rotateSpeed = value; }
		}
		[SerializeField]
		private float
				_maxRotateSpeed;
		public float MaxRotateSpeed {
				get { return _maxRotateSpeed; }
				set { _maxRotateSpeed = value; }
		}
		[SerializeField]
		private float
				_changeSpeedAtRotation;
		public float ChangeSpeedAtRotation {
				get { return _changeSpeedAtRotation; }
				set { _changeSpeedAtRotation = value; }
		}
		[SerializeField]
		private float
				_rotationSpeedIncrement;
		public float RotationSpeedIncrement {
				get { return _rotationSpeedIncrement; }
				set { _rotationSpeedIncrement = value; }
		}
		[SerializeField]
		private float
				_rotationSpeedDecrement;
		public float RotationSpeedDecrement {
				get { return _rotationSpeedDecrement; }
				set { _rotationSpeedDecrement = value; }
		}
		[SerializeField]
		private float
				_jumpPowerIncrement;
		public float JumpPowerIncrement {
				get { return _jumpPowerIncrement; }
				set { _jumpPowerIncrement = value; }
		}
		[SerializeField]
		private float
				_maxAirTime;
		public float MaxAirTime {
				get { return _maxAirTime; }
				set { _maxAirTime = value; }
		}
		/*[SerializeField]
		private float
				_jumpPower;
		public float JumpPower {
				get { return _jumpPower; }
				set { _jumpPower = value; }
		}*/
		[SerializeField]
		private float
				_minJumpPower;
		public float MinJumpPower {
				get { return _minJumpPower; }
				set { _minJumpPower = value; }
		}
		[SerializeField]
		private float
				_maxJumpPower;
		public float MaxJumpPower {
				get { return _maxJumpPower; }
				set { _maxJumpPower = value; }
		}
		[SerializeField]
		private float
				_jumpInterval;
		public float JumpInterval {
				get { return _jumpInterval; }
				set { _jumpInterval = value; }
		}
		[SerializeField]
		private float
				_gravityIncrementWhileInAir;
		public float GravityIncrementWhileInAir {
				get { return _gravityIncrementWhileInAir; }
				set { _gravityIncrementWhileInAir = value; }
		}
		[SerializeField]
		private float
				_maxGravityScale;
		public float MaxGravityScale {
				get { return _maxGravityScale; }
				set { _maxGravityScale = value; }
		}
		/*[SerializeField]
		private float
				_gravityScale;
		public float GravityScale {
				get { return _gravityScale; }
				set { _gravityScale = value; }
		}*/
}