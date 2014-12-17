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

		// Use this for initialization
		void Start ()
		{
				// Cache component lookup at startup instead of doing this every frame		
				thisTransform = GetComponent<Transform> ();
				character = GetComponent<CharacterController> ();	
		
				// Move the character to the correct start position in the level, if one exists
				var spawn = GameObject.Find ("PlayerSpawn");
				if (spawn) {
						thisTransform.position = spawn.transform.position;
				}
		}
	
		// Update is called once per frame
		void Update ()
		{
//				Debug.Log (moveTouchPad.position);
//				Debug.Log (touchPad.position.x + ", is left: " + touchPad.IsFingerOnLeftSide);

				foreach (var touchInfo in touchPad.Touches) {
						var val = touchInfo.Value;
						if (val.Touch.fingerId == 0 && val.IsOnLeftSide) {
								Debug.Log ("Left finger on left side");
								if (val.Touch.tapCount == 2) {
										Debug.Log ("Double Tap on with left finger on left side");
								}
						} else if (val.Touch.fingerId == 0 && !val.IsOnLeftSide) {
								Debug.Log ("Left finger on right side");
								if (val.Touch.tapCount == 2) {
										Debug.Log ("Double Tap on with left finger on right side");
								}
						}
						if (val.Touch.fingerId == 1 && val.IsOnLeftSide) {
								Debug.Log ("Right finger on left side");
								if (val.Touch.tapCount == 2) {
										Debug.Log ("Double Tap on with right finger on left side");
								}
						} else if (val.Touch.fingerId == 1 && !val.IsOnLeftSide) {
								Debug.Log ("Right finger on right side");
								if (val.Touch.tapCount == 2) {
										Debug.Log ("Double Tap on with right finger on right side");
								}
						}
				}
		
				var touchPadPosition = Vector2.zero;
				if (touchPad.Touches.Count == 1) {
						touchPadPosition = touchPad.Touches [0].Position;
//						Debug.Log (touchPad.Touches [0].Id);
				}

				if (touchPad.Touches.Count == 2) {
						var left = touchPad.Touches [0];
						var right = touchPad.Touches [1];
//						Debug.Log (left.Id + ": " + left.Position);
//						Debug.Log (right.Id + ": " + right.Position);
				}
		
		
				//				var movement = thisTransform.TransformDirection (new Vector3 (touchPad.position.x, 0, touchPad.position.y));
				var movement = thisTransform.TransformDirection (new Vector3 (touchPadPosition.x, 0, touchPadPosition.y));
		
				// We only want horizontal movement
				movement.y = 0;
				movement.Normalize ();
				if (touchPad.IsFingerOnLeftSide) {

						
						// Apply movement from move joystick
//			var absJoyPos = new Vector2 (Mathf.Abs (touchPad.position.x), Mathf.Abs (touchPad.position.y));	
						var absJoyPos = new Vector2 (Mathf.Abs (touchPadPosition.x), Mathf.Abs (touchPadPosition.y));	
						if (absJoyPos.y > absJoyPos.x) {

								if (touchPadPosition.y > 0) //(touchPad.position.y > 0)
										movement *= forwardSpeed * absJoyPos.y;
								else
										movement *= backwardSpeed * absJoyPos.y;
						} else {
								movement *= sidestepSpeed * absJoyPos.x;		
						}
				}

				// Check for jump
				if (character.isGrounded) {		
			
						var jump = false;
			
//						if (!moveTouchPad.IsFingerDown ()) {
//								canJump = true;
//						}
			
						if (touchPad.tapCount >= 2 && touchPad.IsFingerOnLeftSide) {
								gameObject.SendMessage ("OnBuildCube", SendMessageOptions.DontRequireReceiver);
								Debug.Log ("build cube");
						}
						if (touchPad.tapCount >= 2 && !touchPad.IsFingerOnLeftSide) {
//								if (canJump) {
								gameObject.SendMessage ("OnJump", SendMessageOptions.DontRequireReceiver);
								Debug.Log ("jump");
								jump = true;
								canJump = false;
//								}
						}
			
						if (jump) {
								// Apply the current movement to launch velocity		
								velocity = character.velocity;
								velocity.y = jumpSpeed;
//								GameObject.FindObjectOfType<Player> ().Jump ();
				
						}
				} else {			
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
		
				// Apply rotation from rotation joystick
//				if (character.isGrounded) 
				{
						var camRotation = Vector2.zero;

//						Debug.Log (rotateTouchPad.position);

						if (touchPad.Touches.Count == 1 && !touchPad.Touches [0].IsOnLeftSide) {
								camRotation = touchPad.Touches [0].Position;
						} else if (touchPad.Touches.Count == 2 && !touchPad.Touches [1].IsOnLeftSide) {
								camRotation = touchPad.Touches [1].Position;
						}

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
			
						camRotation.x *= rotationSpeed.x;
						camRotation.y *= rotationSpeed.y;
						camRotation *= Time.deltaTime;
			
						// Rotate the character around world-y using x-axis of joystick
						thisTransform.Rotate (0, camRotation.x, 0, Space.World);
			
						// Rotate only the camera with y-axis input
						cameraPivot.Rotate (-camRotation.y, 0, 0);
				}
		}

		void OnEndGame ()
		{
				touchPad.Disable ();
				// Don't allow any more control changes when the game ends
				this.enabled = false;
		}
}