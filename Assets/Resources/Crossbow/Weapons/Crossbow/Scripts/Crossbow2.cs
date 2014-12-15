using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Crossbow2 : MonoBehaviour, IWeapon, ICarriable
{
	
		public AudioClip audioShot;
		public bool changeToCamera;
		public Transform arrowPrefab;
		public float weaponRotationX_Aiming;
		public Vector3 weaponPositionRelaxed;
		public float weaponRotationX_Relaxed;
		public Vector3 ZoomInPos;// = new Vector3(); //Position of Zoom
	
		public int Ammo { get; set; }
		public bool CanShoot { get; set; }
		public bool IsActive { get; set; }	
		public Transform Player { get; set; }
		public bool IsAirSliding { get; set; }
		public bool IsSwinging { get; private set; }	
		public bool IsAiming { get; set; }
	
		private Rigidbody WeaponHolderBody { get; set; }
		private CharacterController CharController { get; set; }
		private FPSWalkerEnhanced FpsWalker {
				get { return fpsWalker; }
				set {
						fpsWalker = value;
						useFpsWalker = fpsWalker.enabled;
				}
		}
		private FPSInputController FpsInput { get; set; }
		private MouseLook PlayerMouseLook { get; set; }
		private Camera MainCamera { get; set; }
		private Collider playerCollider;	
		private PlayerTempBody _playerTempRigidbody;
		private Vector3 airVelocityAfterRelease;
		private Vector3 airVelocityBeforeRelease;	
		private Quaternion rotationBeforeRopeSwing;
		private float heightBeforeRopeSwing;	
		private FPSWalkerEnhanced fpsWalker = null;
		private Arrow arrow;
		private Joint joint = null;
		private bool isGrounded = true;
		private bool useFpsWalker = false;	
		private bool canDetachHook = false;
		private bool canUseRope = false;
		private bool cameraChanger = true;
		private Quaternion oldWeaponRotation;
		private Vector3 oldWeaponPosition = Vector3.zero;
		private float motorPower = 1f;
		private bool isSwinging = false;
		private Vector3 ZoomOutPos; //Current Position of Gun
		private float standardFOV;	
		private Vector3 weaponPositionAiming;
	
		void Start ()
		{
				IsActive = false;
				Ammo = 1;
				CanShoot = true;
				IsAirSliding = false;
				IsAiming = false;
				//ChangeCamera();
		}
	
		void FixedUpdate ()
		{
				ProcessInput ();
				ProcessRopeSwing ();
		}
	
		public void SetOwner (Transform player)
		{
				Player = player;
				WeaponHolderBody = player.GetComponentInChildren<Rigidbody> ();
				CharController = player.GetComponent<CharacterController> ();
				FpsWalker = player.GetComponent<FPSWalkerEnhanced> ();
				FpsInput = player.GetComponent<FPSInputController> ();
				PlayerMouseLook = player.GetComponent<MouseLook> ();
				MainCamera = player.GetComponentInChildren<Camera> ();
				Debug.Log (MainCamera);
				playerCollider = Player.GetComponentInChildren<CapsuleCollider> ();
				transform.position = WeaponHolderBody.position;// player.GetComponentInChildren<WeaponHolder>().transform.position;
				transform.rotation = player.rotation;
				weaponPositionAiming = WeaponHolderBody.transform.localPosition; // Assign aiming position from current WH position;
		
				// Aiming
				ZoomOutPos = WeaponHolderBody.transform.localPosition;
				standardFOV = MainCamera.fieldOfView;
		}
	
		public void Fire1 (Vector3 _endPosition, Transform _owner, GameObject _target)
		{
				SpawnArrow (arrowPrefab, _endPosition, _owner, _target);
		}
	
		public void Fire2 ()
		{
				//DestroyArrow();
				AimOrToggleHook ();
		}
	
		public void Reload ()
		{
				//...
		}
	
		void PlaySound (AudioClip clip)
		{
				audio.PlayOneShot (clip);
		}
	
		void SpawnArrow (Transform arrowPrefab, Vector3 _endPosition, Transform _owner, GameObject _target)
		{
				if (CanShoot && IsAiming) {
						DestroyArrow (); // Remove arrows before creating a new one
						PlaySound (audioShot); // Arrow sound
						Transform arrowObject = (Transform)Instantiate (arrowPrefab, this.transform.position, this.transform.rotation); // New arrow
						arrow = arrowObject.GetComponent<Arrow> (); // Create reference to the arrow component
						arrow.Initialize (changeToCamera, WeaponHolderBody, transform.position, _owner, _target); // Setup the arrow
						WeaponHolderBody.constraints = RigidbodyConstraints.FreezeAll;
				}
		}
	
		public void SetRelaxedMode ()
		{
				transform.localPosition = weaponPositionRelaxed;
				//transform.localPosition = ZoomOutPos;
				print ("relaxed mode");
		}
	
		public void SetWeaponArmedMode ()
		{
				//transform.localPosition = weaponPositionRelaxed;
				transform.localPosition = ZoomOutPos;
				print ("relaxed mode");
		}
	
		public void SetAimingMode ()
		{
				//transform.localPosition = weaponPositionAiming;
				transform.localPosition = ZoomInPos; 
//				print ("aiming mode");
		}
	
		void ToggleAim ()
		{
				if (IsAiming) {
						SetRelaxedMode ();
						MainCamera.fieldOfView = standardFOV;
						IsAiming = false;
				} else {
						SetAimingMode ();
						MainCamera.fieldOfView = standardFOV - 20; 
						IsAiming = true;
//						print ("zoom in");
				}
		}
	
		void AimOrToggleHook ()
		{
				if (CharController.isGrounded) {
						if (arrow != null) {
								DestroyArrow ();
						} else {
								ToggleAim ();
						}			
				} else {
						if (canDetachHook) {
								DeactivateRope (); // Stop swinging ("cut rope")
						} else {
								ActivateRope (); // Start swinging
						}
				}
		}
	
		/// <summary>
		/// Processes the rope swing.
		/// </summary>
		void ProcessRopeSwing ()
		{
				if (IsAirSliding) {
						if (GetWeaphonHolder ().HasCollided) {
								//isGrounded = isGrounded ^ true;
								PlayerMouseLook.enabled = true;
								GetWeaponHolderMouseLook ().enabled = false;
								WeaponHolderBody.detectCollisions = false;
								WeaponHolderBody.useGravity = false;
								WeaponHolderBody.freezeRotation = false;
								WeaponHolderBody.isKinematic = true;
								WeaponHolderBody.constraints = RigidbodyConstraints.None;
								WeaponHolderBody.constraints = RigidbodyConstraints.FreezePosition;
								//joint.hingeJoint.connectedBody = null;
								MainCamera.GetComponent<MouseLook> ().axes = MouseLook.RotationAxes.MouseY;
								Player.transform.rotation = rotationBeforeRopeSwing;
								MainCamera.transform.rotation = rotationBeforeRopeSwing;
				
								if (useFpsWalker) {
										fpsWalker.enabled = true;
										fpsWalker.myTransform = Player.transform;
										//fpsWalker.myTransform.position = newPos;
								} else {
										CharController.enabled = true;
										//characterController.transform = Owner.transform;
										//characterController.transform.position = newPos;
								}
								//Player.GetComponentInChildren<MeshRenderer>().enabled = true;
								Player.position = WeaponHolderBody.position + new Vector3 (0f, Player.transform.localScale.y * 2f, 0f); // Set player to be at this new position
								//PlayerMouseLook.transform.rotation = WeaponHolderBody.transform.rotation;
								playerCollider.isTrigger = true; // name: Graphics & Controller
								WeaponHolderBody.transform.rotation = oldWeaponRotation;
								WeaponHolderBody.transform.localPosition = oldWeaponPosition; // 
								ChangeCamera ();
								DestroyArrow (); // Make sure the arrow is destroyed!
								ToggleAim ();
						}
			// Airsliding...
			else {
								//print ("air sliding");
						}
				}
		
				if (arrow != null && arrow.IsHooked) {
						arrow.SetHingeJointRigidBody (WeaponHolderBody);
						arrow.SetRopePoints (WeaponHolderBody.transform.position, arrow.transform.position);
						if (isGrounded) {
								oldWeaponPosition = WeaponHolderBody.transform.localPosition;
								oldWeaponRotation = WeaponHolderBody.transform.localRotation;
						}
				}
		
				if (IsSwinging) {
						WeaponHolderBody.freezeRotation = false;
						//transform.position = WeaponHolderBody.position;
						WeaponHolderBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
				}
		}
	
		void ProcessInput ()
		{
				if (arrow != null) {
						float power = 5f;
						if (Input.GetKeyDown (KeyCode.D)) {
								WeaponHolderBody.AddForce (Vector3.forward * power);
						}
						if (Input.GetKeyDown (KeyCode.A)) {
								WeaponHolderBody.AddForce (Vector3.back * power);
						}
						if (Input.GetKeyDown (KeyCode.W)) {
								WeaponHolderBody.AddForce (Vector3.down * power);
						}
						if (Input.GetKeyDown (KeyCode.S)) {
								WeaponHolderBody.AddForce (Vector3.up * power);
						}
				}
		}
	
		void ActivateRope ()
		{
				if (arrow == null)
						return;
				if (!isGrounded)
						return; // Ignore rope
				//if(fpsWalker.grounded) return;
				if (CharController.isGrounded)
						return;
				//if(!canUseRope) return;
		
				if (fpsWalker.grounded || CharController.isGrounded) {
						rotationBeforeRopeSwing = Player.transform.rotation;
						heightBeforeRopeSwing = Player.transform.position.y;
						//canUseRope = true;
						//IsSwinging = false;
				}
		
		
				isGrounded = isGrounded ^ true;
				//Debug.Log(isGrounded);
				if (!isGrounded) {
						canDetachHook = true;
			
						joint = arrow.GetComponent<Joint> ();
						//print ("type of joint: " + joint.GetType());
						canUseRope = false;
						IsAirSliding = false;
			
						PlayerMouseLook.enabled = true; // Disable player's mouse look
						// disable fps walker or character controller
						if (useFpsWalker) {
								fpsWalker.enabled = false;
						} else {
								CharController.enabled = false;
						}
						// Setup weaponholder which will be swinging
						WeaponHolderBody.detectCollisions = true;
						WeaponHolderBody.useGravity = true;
						WeaponHolderBody.constraints = RigidbodyConstraints.None;
						WeaponHolderBody.isKinematic = false;
						var weaponHolderMouseLook = GetWeaponHolderMouseLook ();
						Debug.Log (weaponHolderMouseLook);
						weaponHolderMouseLook.enabled = true;
						weaponHolderMouseLook.axes = MouseLook.RotationAxes.MouseXAndY;			
						joint.connectedBody = Player.rigidbody;
						WeaponHolderBody.position = Player.position; // Use the player's position
						WeaponHolderBody.constraints = RigidbodyConstraints.FreezeRotationY;
						ChangeCamera ();
						airVelocityBeforeRelease = WeaponHolderBody.velocity;
						playerCollider.isTrigger = false;		
						CanShoot = false;

				} else {
						DestroyArrow ();
				}
		}
	
		void DeactivateRope ()
		{
				if (isGrounded) {
						DestroyArrow ();
						return;
				}
				joint.connectedBody = null;
				WeaponHolderBody.constraints = RigidbodyConstraints.FreezeRotation;
				//WeaponHolderBody.detectCollisions = true;
				//WeaponHolderBody.collider.isTrigger = true;
				DestroyArrow ();
				canDetachHook = false;
				Time.timeScale = 1.0f; // Make sure the game speed is set to 100% again
		
//				print ("weaponholder has collided? " + GetWeaphonHolder ().HasCollided);
				IsAirSliding = true;
		}
	
		void DestroyArrow ()
		{
				if (arrow != null) {
						Destroy (arrow.transform.gameObject);
						arrow = null;
						GetPlayerCamera ().enabled = true;
						CanShoot = true;
				}
		}
	
		/// <summary>
		/// Changes the camera.
		/// Important NOTE:
		/// Do NOT change the camera rotation manually for the player!
		/// </summary>
		public void ChangeCamera ()
		{
				Camera playerCamera = GetPlayerCamera ();
				Camera crossbowCamera = GetCrossbowCamera ();
				// Let the camera on the crossbow which will be ropeswinging have the player's camera rotation
				if (CameraController.instance.isActive) {
						isSwinging = isSwinging ^ cameraChanger;
						print ("isSwinging: " + isSwinging);
						if (isSwinging) {
								CameraController.instance.ChangeCamera (crossbowCamera.name);
						} else {
								CameraController.instance.ChangeCamera (playerCamera.name);
						}
				} else {
						playerCamera.enabled = !(playerCamera.enabled ^ cameraChanger);
						crossbowCamera.enabled = crossbowCamera.enabled ^ cameraChanger;
				}
			
				if (crossbowCamera.enabled) {
						crossbowCamera.transform.rotation = playerCamera.transform.rotation;
						//GetWeaponHolderMouseLook().transform.rotation = PlayerMouseLook.transform.rotation;
				} else {
						//playerCamera.transform.rotation = crossbowCamera.transform.rotation;
				}
		
				//GetWeaponHolderMouseLook().enabled = crossbowCamera.enabled;
		}
	
		private Camera GetPlayerCamera ()
		{
				return Player.GetComponentInChildren<Camera> ();
		}
	
		private Camera GetCrossbowCamera ()
		{
				return GetWeaphonHolder ().transform.GetComponentInChildren<Camera> ();//transform.GetComponent<Camera>();
		}
	
		private MouseLook GetWeaponHolderMouseLook ()
		{
				return GetWeaphonHolder ().transform.GetComponentInChildren<MouseLook> ();
		}
	
		private WeaponHolder GetWeaphonHolder ()
		{
				return Player.GetComponentInChildren<WeaponHolder> ();
		}

	#region ICarriable implementation
		public void Grab ()
		{
				print ("grab");
				//throw new System.NotImplementedException ();
		}
	
		public void Release ()
		{
				DestroyArrow ();
				Destroy (gameObject);
				//throw new System.NotImplementedException ();
		}
	#endregion
}
