using UnityEngine;
using System.Collections;

public class TouchInputController : SpaceCharacterController
{

		private bool hasRegisteredBothFingers = false;

		protected override void FixedUpdate ()
		{
				base.FixedUpdate ();


				
				if (Input.touchCount == 0) {
						if (hasRegisteredBothFingers) {
								Jump ();
								hasRegisteredBothFingers = false;
						}
				} else if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) {
						// Touch and move finger to rotate around a planet

						RotatePlayer (Input.GetTouch (0).deltaPosition);
						//jumpPower = 0f;
				} else if (Input.touchCount == 2) {
						hasRegisteredBothFingers = true;
						gameObject.SendMessage ("OnJumpButtonIsDown", SendMessageOptions.DontRequireReceiver);
						// Charge jump power 
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
				}
		}

		void RotatePlayer (Vector2 touchDeltaPosition)
		{
				if (currentPlanet != null) {

						var horizontalDirection = touchDeltaPosition.x;
						var force = CalculateForce (horizontalDirection);
						if (horizontalDirection != 0f) {
								turnDir = horizontalDirection > 0f ? TurnDir.RIGHT : TurnDir.LEFT;
								gameObject.SendMessage ("OnTurning", force, SendMessageOptions.DontRequireReceiver);
						} else {
								gameObject.SendMessage ("OnSlowingDown", turnDir, SendMessageOptions.DontRequireReceiver);
						}
//						transform.RotateAround (currentPlanet.transform.position, turnDir * Vector3.forward, touchDeltaPosition.magnitude * currentRotationSpeed * Time.deltaTime);
				}
		}
}
