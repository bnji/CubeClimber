using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerExtScr : MonoBehaviour
{
		private Vector3 cameraPositionWalking; // Set automatically in Start()
	
		public float walkSpeed;
		public float useItemRaycastDistance;
		public float weaponRaycastDistance;
		public PlayerData PlayerData { get; set; } // Name, etc, should be refactored to Setup(..) method which will get called from NetworkController!
	
		private CharacterMotor chrMotor;
		private FPSWalkerEnhanced fpsWalker;
		private IUsable usableScript;
		// the current target
		private Transform target;
		private float duration;
		private GameObject usableGameObject;
		private bool isPlayerDead;
		private CameraController cameraController;
		private Transform owner;
		private ICarriable heldItem;
		private Vector3 startPos; // temp for debugging
		private Camera playerCam;
	
		void Awake ()
		{
				var camera = gameObject.GetComponentInChildren<Camera> ();
				if (camera != null) {
						camera.enabled = false;
				}
				var audioListender = gameObject.GetComponentInChildren<AudioListener> ();
				if (audioListender != null) {
						audioListender.enabled = false;
				}
				var charController = gameObject.GetComponent<CharacterController> ();
				if (charController != null) {
						charController.enabled = false;
				}
				var mouseLook = gameObject.GetComponent<MouseLook> ();
				if (mouseLook != null) {
						mouseLook.enabled = false;
				}
				var fpsInputController = gameObject.GetComponent<FPSInputController> ();
				if (fpsInputController != null) {
						fpsInputController.enabled = false;
				}
//				enabled = false;
				
		}
	
		// Use this for initialization
		void Start ()
		{
//				Debug.Log ("start");
				owner = transform.parent;
				// Make sure only the owner of this object can control it
				
				fpsWalker = owner.GetComponent<FPSWalkerEnhanced> ();
				chrMotor = owner.GetComponent<CharacterMotor> ();
				playerCam = owner.GetComponentInChildren<Camera> ();
				cameraPositionWalking = playerCam.transform.localPosition;
			
				Screen.lockCursor = true;
				cameraController = owner.GetComponentInChildren<CameraController> ();
				isPlayerDead = false;
				duration = 1.0f;
				useItemRaycastDistance = 5f;
				weaponRaycastDistance = 10000f;
				startPos = owner.position;
				
				PlayerData = new PlayerData () { Health = 100f, Name = "bendot" };
				
		}
	
		// Update is called once per frame
		void Update ()
		{
//				Debug.Log ("PlayerExtScr update");
				ProcessInput ();
				// we rotate to look at the target every frame (if there is one)
				if (target != null) {
						print (target.name);
						//print ("nearestObj: " + target.name);
						target.renderer.material.color = new Color (1f, 0f, 0f, 1f);
				}
				if (fpsWalker.FellDown) {
						if (!isPlayerDead) {
								Debug.Log ("player died");
//								Transform guiTexture = Resources.Load ("Blood Spatter", typeof(Transform)) as Transform; 
//								Object.Instantiate (guiTexture);
//								//InputController.instance.SetPlayerActive(false);
//								Screen.lockCursor = false;
						}
						isPlayerDead = true;
				}
		}
	
		public WeaponHolder GetWeaponHolder ()
		{
				return transform.parent.GetComponentInChildren<WeaponHolder> ();
		}
	
		ICarriable getHeldItem ()
		{
				return GetWeaponHolder ().CurrentHeldItem;
		}
	
		private float dist; // distance to ground
	
		private void ProcessInput ()
		{
		
				float vScale = 1.0f;
				if (Input.GetButtonDown ("Use")) {
						Use ();
				}
				if (Input.GetButtonDown ("Fire1")) {
						//Use(); // Get grabbable object (if any)
						GetRaycastHit (weaponRaycastDistance);
						heldItem = getHeldItem ();
						if (heldItem != null) {
								/*if(typeof(IWeapon).IsAssignableFrom(heldItem.GetType())) {
					((IWeapon)heldItem).Fire1(raycastHit.point, owner, usableGameObject);
				}*/
								IWeapon weapon = GetWeapon ();
								if (weapon != null) {
										weapon.Fire1 (raycastHit.point, owner, usableGameObject);
								} else {
										heldItem.Grab ();
								}				
						}
				}
				//else if(Input.GetMouseButtonDown(1)) {
				if (Input.GetButtonDown ("Fire2")) {
						heldItem = getHeldItem ();
						if (heldItem != null) {
								/*if(typeof(IWeapon).IsAssignableFrom(heldItem.GetType())) {
					((IWeapon)heldItem).Fire2();
				}*/
								IWeapon weapon = GetWeapon ();
								if (weapon != null) {
										weapon.Fire2 ();
								} else {
										Debug.Log ("released");
										heldItem.Release ();
								}
						}
				}
		}
	
		void OnTriggerStay (Collider other)
		{
		
		}
		
		public IWeapon GetWeapon ()
		{
				if (heldItem != null && typeof(IWeapon).IsAssignableFrom (heldItem.GetType ())) {
						return (IWeapon)heldItem;
				}
				return null;
		}

		void OnDamage (float damage)
		{
				//print("damage: " + damage);
				PlayerData.Health -= damage;
				Debug.Log ("Health : " + PlayerData.Health);
		
				if (PlayerData.Health <= 0f) {
						Destroy (owner.gameObject);
						Application.LoadLevelAsync (Application.loadedLevel);
				}
		}
	
		void ScanForTarget ()
		{
				// this should be called less often, because it could be an expensive
				// process if there are lots of objects to check against
				target = GetNearestTaggedObject ();
		}
	
		private Transform GetNearestTaggedObject ()
		{
				// and finally the actual process for finding the nearest object:
	 
				var nearestDistanceSqr = Mathf.Infinity;
				var taggedGameObjects = GameObject.FindObjectsOfType (typeof(MonoBehaviour));
				if (taggedGameObjects != null)
						Debug.Log ("go len: " + taggedGameObjects.Length);
				Transform nearestObj = null;
	 
				// loop through each tagged object, remembering nearest one found
				foreach (GameObject obj in taggedGameObjects) {
						Debug.Log (obj.name);
						var objectPos = obj.transform.position;
						var distanceSqr = (objectPos - owner.position).sqrMagnitude;
	 
						if (distanceSqr < nearestDistanceSqr) {
								nearestObj = obj.transform;
								nearestDistanceSqr = distanceSqr;
						}
				}
				return nearestObj;
		}
	
		void OnLand (Collision collision)
		{
				print ("player landed (collided with) on " + collision.collider.name);
		}
	
		private void GetRaycastHit (float _rayCastDistance)
		{
				RaycastHit _rayCastHit = new RaycastHit ();
				Camera c = gameObject.GetComponentInChildren<Camera> ();
				Ray ray;
				if (cameraController != null && cameraController.isActive) {				
						ray = cameraController.ActiveCamera.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2));
						Physics.Raycast (ray, out _rayCastHit, _rayCastDistance);
				} else {
						ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2));
						Physics.Raycast (ray, out _rayCastHit, _rayCastDistance);
				}
				this.raycastHit = _rayCastHit;
				if (this.raycastHit.collider != null) {
//						Debug.Log (this.raycastHit.collider);
						this.usableGameObject = this.raycastHit.collider.gameObject;
				}
		}
	
		RaycastHit raycastHit = new RaycastHit ();
	
		private void Use ()
		{
				GetRaycastHit (useItemRaycastDistance);
				if (usableGameObject != null) {
						IUsable _usableScript = (IUsable)usableGameObject.GetComponent (typeof(IUsable));
						if (_usableScript != null) {
								usableScript = _usableScript;
						}
						//string isUsable = _usableScript != null ? "YES!" : "";
						//string isGrabbable = grabbableScript != null ? "YES!" : "";			
						//print ("The gameObject: " + usableGameObject.name + " is " + Mathf.Round(raycastHit.distance) + " meters away from you. usable? " + isUsable + ", grabbable? " + isGrabbable);
				}
				if (usableScript != null) {
						usableScript.Use (owner, raycastHit);
				}
		}
	
}
