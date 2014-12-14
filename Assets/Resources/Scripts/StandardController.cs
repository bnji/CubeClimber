using UnityEngine;
using System.Collections;

public class StandardController : SpaceCharacterController
{
	
		private float localRotationInAir = 0f;
	
		public void RotateInAir ()
		{
				// Rotate player while in air
				localRotationInAir += ControllerInfo.RotateSpeedWhileInAir * -Input.GetAxis ("Horizontal") * Time.deltaTime;						
				transform.Rotate (new Vector3 (0f, 0f, localRotationInAir));
		}
	
		// Update is called once per frame
		protected override void FixedUpdate ()
		{
				base.FixedUpdate ();
		
		
				//Debug.Log (rotateDirectionOnDecrease + " - " + Input.GetAxis ("Horizontal"));

				var horizontalDirection = Input.GetAxis ("Horizontal");
				var verticalDirection = Input.GetAxis ("Vertical");
				var force = CalculateForce (horizontalDirection * verticalDirection);
				//						Debug.Log (force.magnitude);
				if (horizontalDirection != 0f) {
						turnDir = horizontalDirection < 0f ? TurnDir.RIGHT : TurnDir.LEFT;
						gameObject.SendMessage ("OnTurning", force, SendMessageOptions.DontRequireReceiver);
				} else {
						gameObject.SendMessage ("OnSlowingDown", turnDir, SendMessageOptions.DontRequireReceiver);
				}
				localRotationInAir = 0f;

//				if (currentPlanet != null) {
//						var horizontalDirection = Input.GetAxis ("Horizontal");
//						var force = CalculateForce (horizontalDirection);
//						//						Debug.Log (force.magnitude);
//						if (horizontalDirection != 0f) {
//								turnDir = horizontalDirection < 0f ? TurnDir.RIGHT : TurnDir.LEFT;
//								gameObject.SendMessage ("OnTurning", force, SendMessageOptions.DontRequireReceiver);
//						} else {
//								gameObject.SendMessage ("OnSlowingDown", turnDir, SendMessageOptions.DontRequireReceiver);
//						}
//						localRotationInAir = 0f;
//				} else {
//						if (Input.GetAxis ("Horizontal") == 0f) {
//								if (localRotationInAir > 0f) {
//										localRotationInAir -= -ControllerInfo.RotateSpeedWhileInAir * Time.deltaTime;	
//										transform.Rotate (new Vector3 (0f, 0f, localRotationInAir));	
//								}
//						} else {
//								//Rotation is to be handled in the method OnHorizontalPressedWhileInAir
//								gameObject.SendMessage ("OnHorizontalPressedWhileInAir", localRotationInAir, SendMessageOptions.DontRequireReceiver);
//						}
//			
//						if (Input.GetAxis ("Vertical") == 0f) {
//				
//						} else {
//								gameObject.SendMessage ("OnVerticalPressedWhileInAir", SendMessageOptions.DontRequireReceiver);
//						}
//			
//				}
				// Consume enough jump power while holding down the space key 
				if (Input.GetButton ("Jump")) {
						gameObject.SendMessage ("OnJumpButtonIsDown", SendMessageOptions.DontRequireReceiver);
						// Make sure the player doesn't get too much jump power and only
						// can consume jump power while on a planet (not in "air").
						if (jumpPowerDirection && JumpPower <= ControllerInfo.MaxJumpPower && !isInAir && currentPlanet != null) {
								JumpPower += ControllerInfo.JumpPowerIncrement;
								gameObject.SendMessage ("OnJumpButtonPressed", SendMessageOptions.DontRequireReceiver);
						} else {
								jumpPowerDirection = false;
						}
						if (!jumpPowerDirection && JumpPower >= 0f && !isInAir && currentPlanet != null) {
								JumpPower -= ControllerInfo.JumpPowerIncrement;
								gameObject.SendMessage ("OnJumpButtonPressed", SendMessageOptions.DontRequireReceiver);
						} else {
								jumpPowerDirection = true;
						}
			
						if (isInAir && currentPlanet == null) {
								gameObject.SendMessage ("OnJumpButtonPressedWhileInAir", SendMessageOptions.DontRequireReceiver);
				
						}
				} else {
						Jump ();
				}
		
		}
}
