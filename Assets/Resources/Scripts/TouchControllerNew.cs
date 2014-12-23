using UnityEngine;
using System.Collections;

//////////////////////////////////////////////////////////////
// FirstPersonControl.js
//
// FirstPersonControl creates a control scheme where the camera 
// location and controls directly map to being in the first person.
// The left pad is used to move the character, and the
// right pad is used to rotate the character. A quick double-tap
// on the right joystick will make the character jump.
//
// If no right pad is assigned, then tilt is used for rotation
// you double tap the left pad to jump
//////////////////////////////////////////////////////////////
[RequireComponent(typeof(CharacterController))]
public class TouchControllerNew : MonoBehaviour
{
		public bool isDebugMode = false;
		// This script must be attached to a GameObject that has a CharacterController
		public JoystickNew moveTouchPad;
		public JoystickNew touchPad;						// If unassigned, tilt is used
	
		public Transform cameraPivot;						// The transform used for camera rotation
	
		public float forwardSpeed = 4;
		public float backwardSpeed = 1;
		public float sidestepSpeed = 1;
		public float jumpSpeed = 8;
		public float inAirMultiplier = 0.25f;					// Limiter for ground speed while jumping
		public Vector2 rotationSpeed = new Vector2 (50f, 25f);	// Camera rotation speed for each axis
		public float tiltPositiveYAxis = 0.6f;
		public float tiltNegativeYAxis = 0.4f;
		public float tiltXAxisMinimum = 0.1f;
	
		private Transform thisTransform;
		private CharacterController character;
		private Vector3 cameraVelocity;
		private Vector3 velocity;						// Used for continuing momentum while in air
		private bool canJump = true;

		// Apply movement from move joystick
		Vector3 ApplyMovement (Vector3 movement, Vector2 touchPadPosition)
		{
				var absJoyPos = new Vector2 (Mathf.Abs (touchPadPosition.x), Mathf.Abs (touchPadPosition.y));	
				if (absJoyPos.y > absJoyPos.x) {
						if (touchPadPosition.y > 0) //(touchPad.position.y > 0)
								movement *= forwardSpeed * absJoyPos.y;
						else
								movement *= backwardSpeed * absJoyPos.y;
				} else {
						movement *= sidestepSpeed * absJoyPos.x;		
				}
				return movement;
		}

		void Build ()
		{
				gameObject.SendMessage ("OnBuildCube", SendMessageOptions.DontRequireReceiver);
				Debug.Log ("build cube");
		}
	
		void Jump ()
		{
				gameObject.SendMessage ("OnJump", SendMessageOptions.DontRequireReceiver);
				Debug.Log ("jump");
//				jump = true;
//				canJump = true;
		}
	
		// Use this for initialization
		void Start ()
		{
				lastTime = Time.time;
				// Cache component lookup at startup instead of doing this every frame		
				thisTransform = GetComponent<Transform> ();
				character = GetComponent<CharacterController> ();	
		
				// Move the character to the correct start position in the level, if one exists
				var spawn = GameObject.Find ("PlayerSpawn");
				if (spawn) {
						thisTransform.position = spawn.transform.position;
				}

				debugText.gameObject.SetActive (isDebugMode);
				debugText2.gameObject.SetActive (isDebugMode);
				//				debugText = GameObject.FindObjectOfType<DebugText> ();
		}

		void RotatePlayer (Vector3 camRotation)
		{
				// Apply rotation from rotation joystick
				//if (character.isGrounded) 
				{
//			var camRotation = val.Position;
			
						//						if (!touchPad.IsFingerOnLeftSide) {
						//								if (touchPad.Touches.Count == 1) {
						//										camRotation = touchPad.Touches [0].Position;// touchPad.position;
						//								}
						//						} else {
						//								//								// Use tilt instead
						////								//			print( iPhoneInput.acceleration );
						////								var acceleration = Input.acceleration;
						////								var absTiltX = Mathf.Abs (acceleration.x);
						////								if (acceleration.z < 0 && acceleration.x < 0) {
						////										if (absTiltX >= tiltPositiveYAxis)
						////												camRotation.y = (absTiltX - tiltPositiveYAxis) / (1 - tiltPositiveYAxis);
						////										else if (absTiltX <= tiltNegativeYAxis)
						////												camRotation.y = -(tiltNegativeYAxis - absTiltX) / tiltNegativeYAxis;
						////								}
						////				
						////								if (Mathf.Abs (acceleration.y) >= tiltXAxisMinimum)
						////										camRotation.x = -(acceleration.y - tiltXAxisMinimum) / (1 - tiltXAxisMinimum);
						//						}
			
						//										Debug.Log (camRotation);
						camRotation.x *= rotationSpeed.x;
						camRotation.y *= rotationSpeed.y;
						camRotation *= Time.deltaTime;
			
						// Rotate the character around world-y using x-axis of joystick
						thisTransform.Rotate (0, camRotation.x, 0, Space.World);
			
						// Rotate only the camera with y-axis input
						cameraPivot.Rotate (-camRotation.y, 0, 0);
				}
		}

		public DebugText debugText;
		public DebugText debugText2;
		int fingerId;
	
		TouchInfo touchMove = null;
		TouchInfo touchRotate = null;

		bool isTouchInitialTouchLeftSide = false;
		bool isTouchInitialTouchRightSide = false;
	
		// Update is called once per frame
		void FixedUpdate ()
		{
		
				var movement = Vector3.zero;

				if (touchPad.Touches.Count == 0) {
						touchMove = null;
						touchRotate = null;
				}
		
				foreach (var touchInfo in touchPad.Touches) {

						var touch = touchInfo.Value;

						fingerId = touch.Touch.fingerId;
						
//						if (touch.Touch.phase == TouchPhase.Began) {
//								debugText.SetText ("" + fingerId + ", left?: " + touch.IsOnLeftSide);
//						}

//						if (touch.Touch.position.x > Screen.width - 200 && touch.Touch.position.y > Screen.height - 200) {
//								debugText.SetText ("OK");
//						}
//						debugText.SetText (touch.Touch.position.x + "/" + Screen.width);

						if (fingerId == 0) {

								// if finger initially is touched on left side then set it to be touch move
								// if finger initially is touched on right side then set it to be touch rotate
								// on touch end set touch move and touchrotate to null
								isTouchInitialTouchLeftSide = touch.Touch.phase == TouchPhase.Began && touch.IsOnLeftSide;
								isTouchInitialTouchRightSide = touch.Touch.phase == TouchPhase.Began && !touch.IsOnLeftSide;
				
								if (isTouchInitialTouchLeftSide) {
										touchMove = touch;
										//move
								}
								if (isTouchInitialTouchRightSide) {
										touchRotate = touch;
								}

								if (touch.Touch.phase == TouchPhase.Ended) {
										touchMove = null;
										touchRotate = null;
								}
						}
						if (fingerId == 1) {
								touchRotate = touch;
						}
			
//						if (touch.Touch.fingerId == 0 && touch.IsOnLeftSide) {
//								Debug.Log ("Left finger on left side");
//				
//								if (touch.Touch.phase != TouchPhase.Began) {
//										movement = thisTransform.TransformDirection (new Vector3 (touch.Position.x, 0, touch.Position.y));
//					
//										// We only want horizontal movement
//										movement.y = 0;
//										movement.Normalize ();
//										movement = ApplyMovement (movement, touch.Position);
//								}
//								if (touch.Touch.tapCount == 2) {
//										if (Helper.IsTimeUp (150f))
//												Build ();
//										Debug.Log ("Double Tap on with left finger on left side");
//								}
//						} else if (touch.Touch.fingerId == 0 && !touch.IsOnLeftSide) {
//								Debug.Log ("Left finger on right side. Touch phase: " + touch.Touch.phase);
//								if (touch.Touch.phase == TouchPhase.Moved) {
//										RotatePlayer (touch.Position);
//								}
//								if (touch.Touch.tapCount == 2) {
////										if (Helper.IsTimeUp (250f))
//										if (canJump && character.isGrounded) {
//												// Apply the current movement to launch velocity		
//												velocity = character.velocity;
//												velocity.y = jumpSpeed;
//												Jump ();
//												canJump = false;
//												lastTime = Time.time;
//										}
//										Debug.Log ("Double Tap on with left finger on right side");
//								}
//						}
//						if (touch.Touch.fingerId == 1 && touch.IsOnLeftSide) {
//								Debug.Log ("Right finger on left side");
//								if (touch.Touch.tapCount == 2) {
//										Debug.Log ("Double Tap on with right finger on left side");
//								}
//						} else if (touch.Touch.fingerId == 1 && !touch.IsOnLeftSide) {
//								if (touch.Touch.phase == TouchPhase.Moved) {
//										Debug.Log ("Right finger on right side");
//										RotatePlayer (touch.Position);
//								}
//								if (touch.Touch.tapCount == 2) {
//										Jump ();
//										Debug.Log ("Double Tap on with right finger on right side");
//								}
//						}
				}

				if (touchRotate != null) {
						if (touchRotate.Touch.phase == TouchPhase.Moved) {
								RotatePlayer (touchRotate.Position);
						}
//						Debug.Log ("" + (touchRotate.Touch.position.x > Screen.width - 200));
						
//						Debug.Log (item1Distance + " - " + touchRotate.Touch.position.x + "/" + Screen.width * item1.position.x);
						if (touchRotate.Touch.phase == TouchPhase.Began && IsItemHit (touchRotate, item1)) {// touchRotate.Touch.position.x > Screen.width - 64 && touchRotate.Touch.position.y > Screen.height - 64) {
								gameObject.SendMessage ("ToggleAutoBuild", item1, SendMessageOptions.DontRequireReceiver);
						}
						if (touchRotate.Touch.phase == TouchPhase.Began && IsItemHit (touchRotate, item2)) {
								gameObject.SendMessage ("TogglePause", item2, SendMessageOptions.DontRequireReceiver);
						}
						if (touchRotate.Touch.tapCount == 2) {
								//										if (Helper.IsTimeUp (250f))
								if (canJump && character.isGrounded) {
										// Apply the current movement to launch velocity		
										velocity = character.velocity;
										velocity.y = jumpSpeed;
										Jump ();
										canJump = false;
										lastTime = Time.time;
								}
								Debug.Log ("Double Tap on with left finger on right side");
						}
						if (isDebugMode) {
								debugText2.SetText ("rotate: pos: " + touchRotate.Touch.position + ", fid: " + touchRotate.Touch.fingerId + ", left?: " + touchRotate.IsOnLeftSide);
						}
				} else {
						if (isDebugMode) {
								debugText2.SetText ("");
						}
				}
		
				if (touchMove != null) {
						if (touchMove.Touch.phase != TouchPhase.Began) {
								movement = thisTransform.TransformDirection (new Vector3 (touchMove.Position.x, 0, touchMove.Position.y));
				
								// We only want horizontal movement
								movement.y = 0;
								movement.Normalize ();
								movement = ApplyMovement (movement, touchMove.Position);
						}
						if (touchMove.Touch.tapCount == 2) {
								if (Helper.IsTimeUp (150f))
										Build ();
								Debug.Log ("Double Tap on with left finger on left side");
						}
			
						if (isDebugMode) {
								debugText.SetText ("move: pos: " + touchMove.Touch.position + ", fid: " + touchMove.Touch.fingerId + ", left?: " + touchMove.IsOnLeftSide);
						}
				} else {
			
						if (isDebugMode) {
								debugText.SetText ("");
						}
				}
		
		
				if (!character.isGrounded) {
						// Apply gravity to our velocity to diminish it over time
						velocity.y += Physics.gravity.y * Time.deltaTime;
			
						// Adjust additional movement while in-air
						movement.x *= inAirMultiplier;
						movement.z *= inAirMultiplier;
				}
		
				movement += velocity;	
				movement += Physics.gravity;
				movement *= Time.deltaTime;
		
				// Actually move the character	
				character.Move (movement);

		
		
				if (character.isGrounded) {
						// Remove any persistent velocity after landing	
						velocity = Vector3.zero;
				}

				if (IsTimeUp (250f)) {
						canJump = true;
				}
		}

		bool IsItemHit (TouchInfo touchInfo, Transform item, float tolerance = 48f)
		{
				var dist = Vector2.Distance (touchInfo.Touch.position, new Vector2 (Screen.width * item.position.x, Screen.height * item.position.y));
				Debug.Log ("dist: " + dist + ", item: " + item.name);
				return dist < tolerance;
		}

		public Transform item1;
		public Transform item2;

		private float lastTime;
	
		public bool IsTimeUp (float timeLimit)
		{
				if ((Time.time - lastTime) * 1000f > timeLimit) {
						lastTime = Time.time;
						return true;
				}
				return false;
		}
	
		void OnEndGame ()
		{
				touchPad.Disable ();
				// Don't allow any more control changes when the game ends
				this.enabled = false;
		}
}