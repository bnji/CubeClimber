using UnityEngine;
using System.Collections;

public class Player : PlanetCitizen, IPause, IDestroyable// MonoBehaviour
{
		public Object cubePrefab;
		public Transform CurrentMainCube { get; set; }
		public Transform CurrentInvisibleCube { get; set; }

		public AudioClip jumpSound;
		public AudioClip landedSound;
		public AudioClip buildCubeSound;
	
	#region IDestroyable implementation
	
		public void Destroy ()
		{
				Debug.Log ("GAME OVER");
//		RestartGame ();
		}
	
	#endregion

		void Start ()
		{
		}

		void OnJump ()
		{
				AudioSource.PlayClipAtPoint (jumpSound, transform.position);


				if (CurrentMainCube != null) {
						var currentCubeScript = CurrentMainCube.GetComponent<Cube> ();
						var cubeTriggers = currentCubeScript.GetComponentsInChildren<CubeTrigger> ();
						foreach (var item in cubeTriggers) {
								item.CanShowInvisibleCube = false;
						}
						foreach (Transform child in CurrentMainCube.transform) {
								GameObject.Destroy (child.gameObject);
						}
						var rb = CurrentMainCube.GetComponent<Rigidbody> ();
						if (rb != null) {
								rb.isKinematic = false;
								rb.useGravity = true;
						}
//						currentCubeScript.Destroy ();
				}
		}

		void OnLand ()
		{
				Debug.Log ("landed");
				if (landedSound != null) {
						AudioSource.PlayClipAtPoint (landedSound, transform.position);
				}
		}

		void OnFall ()
		{
//				print ("Falling");
		}

		public void BuildCube ()
		{
				if (CurrentInvisibleCube == null)
						return;

				var currentInvisibleCubeScript = CurrentInvisibleCube.GetComponent<CubeInvisible> ();
				if (currentInvisibleCubeScript.BuildCube (cubePrefab)) {
						AudioSource.PlayClipAtPoint (buildCubeSound, transform.position);
						Debug.Log ("Cube count: " + GameObject.FindObjectOfType<GameHandler> ().CubePositions.Count);
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (Input.GetKeyDown (KeyCode.E) && Helper.IsTimeUp (250f)) {
						BuildCube ();
				}

//				Vector3 fwd = transform.TransformDirection (Vector3.forward);
//				RaycastHit rayCastHit;
//		
//				Ray ray = Camera.main.ViewportPointToRay (Input.mousePosition);
//				if (Physics.Raycast (transform.position, fwd, out rayCastHit, 10000)) {
//						Debug.DrawLine (ray.origin, rayCastHit.point);
//						var cubeHit = rayCastHit.transform.GetComponent<Cube> ();
//						if (cubeHit != null) {
//								print (cubeHit.transform.name);
//						}
////						print (rayCastHit.transform.name);
//				}
		}

		public void ApplyDamage (float damage)
		{
				data.Health -= damage;
				//Debug.Log ("Player health: " + health);
				// Update the health bar
//				UpdateHealthBar ();
		}
	
	
		public float IncreaseHealth (float _health)
		{
				return UpdateHealth (_health);
		}
	
		public float DecreaseHealth (float _health)
		{
				return UpdateHealth (-_health);
		}
	
		private float UpdateHealth (float _health)
		{
				float newHealth = Mathf.Clamp (data.Health + _health, 0f, data.MaxHealth);
				data.Health = newHealth;
				if (data.Health <= 0f) {
						Debug.Log ("Player died");
//						RestartGame ();
				} else {
//						UpdateHealthBar ();
						PlayerPrefs.SetFloat ("Health", data.Health);
				}
				return newHealth;
		}
	
		public void IncreaseScore (float _score)
		{
				UpdateScore (_score);
		}
	
		private void UpdateScore (float _score)
		{
				data.Score += _score;
				// Save the score if it's larger than before
				if (PlayerPrefs.GetFloat ("Score") < data.Score) {
						PlayerPrefs.SetFloat ("Score", data.Score);
						Debug.Log ("player score increased");
//						UpdateScoreHUD ();
				}
		}

		public float maxTurnForceMagnitude = 2f;// 0.0007f;
		public float slowingDownDampingFactor = 0.01f;
		public float maxVelocityBeforeCanSlowDown = 1f;
	
		public ControllerActionType _controllerActionType = ControllerActionType.ROTATE;
	
		public void SetControllerMovement (ControllerActionType controllerActionType)
		{
				_controllerActionType = controllerActionType;
		}

		void OnVerticalPressedWhileInAir ()
		{
				if (_controllerActionType == ControllerActionType.MOVE) {
						float dir = Input.GetAxis ("Vertical");
						rigidbody.AddForce (transform.up * dir * 0.1f * Time.deltaTime);
			
//						if (dir > 0f) {
//								GetComponent<Jetpack> ().Activate (JetpackDirection.BOTTOM);
//						} else {
//								GetComponent<Jetpack> ().Activate (JetpackDirection.TOP);
//						}
			
				}
		}
	
		void OnHorizontalPressedWhileInAir (float localRotationInAir)
		{
				switch (_controllerActionType) {
				case ControllerActionType.ROTATE:
						(controller as GravityController).RotateInAir ();
						break;
				case ControllerActionType.MOVE:
						{
								float dir = Input.GetAxis ("Horizontal");
								rigidbody.AddForce (transform.right * dir * 0.1f * Time.deltaTime);
//								if (dir > 0f) {
//										GetComponent<Jetpack> ().Activate (JetpackDirection.LEFT);
//								} else {
//										GetComponent<Jetpack> ().Activate (JetpackDirection.RIGHT);
//								}
						}
						break;
				}
		}

		void OnTurning (Vector3 force)
		{
				if (rigidbody.velocity.sqrMagnitude < maxTurnForceMagnitude) {
						rigidbody.AddForce (force, controller.forceMode);
				}
		}
	
//		void OnSlowingDown (TurnDir turnDir)
//		{
//				if (rigidbody2D.velocity.sqrMagnitude < maxVelocityBeforeCanSlowDown)
//						return;
//		
//				switch (turnDir) {
//				case TurnDir.LEFT:
//			//						debugText.SetText ("turning left");
//			//						GetComponent<Jetpack> ().Activate (JetpackDirection.LEFT);
//						rigidbody.AddForce (transform.right * slowingDownDampingFactor * Time.deltaTime, controller.forceMode);
//						break;
//				case TurnDir.RIGHT:
//			//						debugText.SetText ("turning right");
//			//						GetComponent<Jetpack> ().Activate (JetpackDirection.RIGHT);
//						rigidbody.AddForce (transform.right * -slowingDownDampingFactor * Time.deltaTime, controller.forceMode);
//						break;
//				}
//		}
}
