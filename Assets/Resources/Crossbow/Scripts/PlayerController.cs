//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//[RequireComponent (typeof(CharacterController))]
//public class PlayerController : MonoBehaviour
//{
//	
//		private FPSWalkerEnhanced fpsWalker = null;
//		public PlayerData PlayerData { get; set; } // Name, etc, should be refactored to Setup(..) method which will get called from NetworkController!
//		public float rayCastDistance = 5.25f;
//		public float weaponHitDistance = 1000f;
//		private IUsable usableScript;
//		// the current target
//		private Transform target;
//		private float duration = 1.0f;
//		private GameObject usableGameObject;
//		private bool isPlayerDead = false;
//		private CameraController cameraController;
//	
//		//public PlayerWeaponController WeaponController;
//		//public WeaponHolder Weapon { get; set; }
//	
//		void Awake ()
//		{
//		
//				fpsWalker = GetComponent<FPSWalkerEnhanced> ();
//		
//				// Prevent the object from being destroyed when loading a different scene (level)
//				DontDestroyOnLoad (this);
//
//				//We aren't the network owner, disable this script
//				//RPC's and OnSerializeNetworkView will STILL get trough!
//				gameObject.GetComponentInChildren<Camera> ().enabled = false;
//				gameObject.GetComponentInChildren<AudioListener> ().enabled = false;
//				gameObject.GetComponent<CharacterController> ().enabled = false;
//				gameObject.GetComponent<MouseLook> ().enabled = false;
//				gameObject.GetComponent<FPSInputController> ().enabled = false;
//				//gameObject.GetComponent<Camera>().enabled = false;
//				enabled = false;
//				//InputController.instance.SetPlayerActive(false);
//		}
//	
//		// Use this for initialization
//		void Start ()
//		{
//				Screen.lockCursor = true;
//				cameraController = GetComponentInChildren<CameraController> ();
//				
////						PlayerData = new PlayerData () { Health = 100f, Name = "bendot" };
//				
//		
//		}
//	
//		// Update is called once per frame
//		void Update ()
//		{
//				ProcessInput ();
//				// we rotate to look at the target every frame (if there is one)
//				if (target != null) {
//						print (target.name);
//						//print ("nearestObj: " + target.name);
//						target.renderer.material.color = new Color (1f, 0f, 0f, 1f);
//				}
//				if (fpsWalker.FellDown) {
//						if (!isPlayerDead) {
//								Transform guiTexture = Resources.Load ("Blood Spatter", typeof(Transform)) as Transform; 
//								Object.Instantiate (guiTexture);
//								//InputController.instance.SetPlayerActive(false);
//								Screen.lockCursor = false;
//						}
//						isPlayerDead = true;
//				}
//		}
//	
//		private ICarriable heldItem;
//		//private float posYbeforeJumping = -999;
//		//private bool isPosYLocked = false;
//	
//		private void ProcessInput ()
//		{
//				if (Input.GetKeyDown (KeyCode.E)) {
//						Use ();
//				} else if (Input.GetMouseButtonDown (0)) {
//						//Use(); // Get grabbable object (if any)
//						GetRaycastHit (weaponHitDistance);
//						heldItem = getHeldItem ();
//						if (heldItem != null) {
//								if (typeof(IWeapon).IsAssignableFrom (heldItem.GetType ())) {
//										((IWeapon)heldItem).Fire1 (raycastHit.point, transform, usableGameObject);
//								} else {
//										heldItem.Grab ();
//								}				
//						}
//				} else if (Input.GetMouseButtonDown (1)) {
//						heldItem = getHeldItem ();
//						if (heldItem != null) {
//								if (typeof(IWeapon).IsAssignableFrom (heldItem.GetType ())) {
//										((IWeapon)heldItem).Fire2 ();
//								} else {
//										heldItem.Release ();
//								}
//						}
//				}
//		}
//	
//		ICarriable getHeldItem ()
//		{
//				return transform.GetComponentInChildren<WeaponHolder> ().CurrentHeldItem;
//		}
//	
//		void OnDamage (float damage)
//		{
//				PlayerData.Health -= damage;
//				Debug.Log ("Health : " + PlayerData.Health);
//		
//				if (PlayerData.Health <= 0f) {
//						Destroy (gameObject);
//						Application.LoadLevel (Application.loadedLevel);
//				}
//		}
//	
//		void ScanForTarget ()
//		{
//				// this should be called less often, because it could be an expensive
//				// process if there are lots of objects to check against
//				target = GetNearestTaggedObject ();
//		}
//	
//		private Transform GetNearestTaggedObject ()
//		{
//				// and finally the actual process for finding the nearest object:
//	 
//				var nearestDistanceSqr = Mathf.Infinity;
//				var taggedGameObjects = GameObject.FindObjectsOfType (typeof(MonoBehaviour));
//				if (taggedGameObjects != null)
//						Debug.Log ("go len: " + taggedGameObjects.Length);
//				Transform nearestObj = null;
//	 
//				// loop through each tagged object, remembering nearest one found
//				foreach (GameObject obj in taggedGameObjects) {
//						Debug.Log (obj.name);
//						var objectPos = obj.transform.position;
//						var distanceSqr = (objectPos - transform.position).sqrMagnitude;
//	 
//						if (distanceSqr < nearestDistanceSqr) {
//								nearestObj = obj.transform;
//								nearestDistanceSqr = distanceSqr;
//						}
//				}
//				return nearestObj;
//		}
//	
//		void SendPos ()
//		{
//				networkView.RPC ("SendPosition", RPCMode.OthersBuffered, transform.position);
//		}
//	
//		private void GetRaycastHit (float _rayCastDistance)
//		{
//				RaycastHit _rayCastHit = new RaycastHit ();
//				Camera c = gameObject.GetComponentInChildren<Camera> ();
//				//Physics.Raycast(c.transform.position, c.transform.TransformDirection (Vector3.forward), out raycastHit, rayCastDistance);
//				//Physics.Raycast(Camera.mainCamera.transform.position, Camera.mainCamera.transform.forward, out raycastHit, rayCastDistance);
//		
//				//Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
//				//Physics.Raycast(ray, out _rayCastHit, _rayCastDistance);
//		
//				//Physics.Raycast(cameraController.mainCamera.transform.position, cameraController.mainCamera.transform.forward, out _rayCastHit, rayCastDistance);
//				//Physics.Raycast(cameraController.ActiveCamera.transform.position, cameraController.ActiveCamera.transform.forward, out rh, rayCastDistance);
//		
//		
//				Ray ray = cameraController.ActiveCamera.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2));
//				Physics.Raycast (ray, out _rayCastHit, _rayCastDistance);
//		
//				this.raycastHit = _rayCastHit;
//				this.usableGameObject = this.raycastHit.collider.gameObject;
//		
//		}
//	
//		RaycastHit raycastHit = new RaycastHit ();
//	
//		private void Use ()
//		{
//				GetRaycastHit (rayCastDistance);
//				if (usableGameObject != null) {
//						IUsable _usableScript = (IUsable)usableGameObject.GetComponent (typeof(IUsable));
//						if (_usableScript != null) {
//								usableScript = _usableScript;
//						}
//						//string isUsable = _usableScript != null ? "YES!" : "";
//						//string isGrabbable = grabbableScript != null ? "YES!" : "";			
//						//print ("The gameObject: " + usableGameObject.name + " is " + Mathf.Round(raycastHit.distance) + " meters away from you. usable? " + isUsable + ", grabbable? " + isGrabbable);
//				}
//				if (usableScript != null) {
//						usableScript.Use (transform, raycastHit);
//				}
//		}
//	
//	
//}
