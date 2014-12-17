using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// A simple class for bounding how far the GUITexture will move
public class Boundary
{
		public Vector2 min = Vector2.zero;
		public Vector2 max = Vector2.zero;
}

public class TouchInfo
{
		public Touch Touch {
				get;
				set;
		}
		public int Id {
				get;
				set;
		}
		public Vector2 Position {
				get;
				set;
		}
		public bool IsOnLeftSide {
				get;
				set;
		}
		public TouchInfo ()
		{
		}
}

//////////////////////////////////////////////////////////////
// Joystick.js
// Penelope iPhone Tutorial
//
// Joystick creates a movable joystick (via GUITexture) that 
// handles touch input, taps, and phases. Dead zones can control
// where the joystick input gets picked up and can be normalized.
//
// Optionally, you can enable the touchPad property from the editor
// to treat this Joystick as a TouchPad. A TouchPad allows the finger
// to touch down at any point and it tracks the movement relatively 
// without moving the graphic
//////////////////////////////////////////////////////////////
public class JoystickNew : MonoBehaviour
{
		static private JoystickNew[] joysticks;					// A static collection of all joysticks
		static private bool enumeratedJoysticks = false;
		static private float tapTimeDelta = 0.3f;				// Time allowed between taps
	
		public Rect touchZone;
		public Vector2 deadZone = Vector2.zero;						// Control when position is output
		public bool normalize = false; 							// Normalize output after the dead-zone?
		public Vector2 position; 									// [-1, 1] in x,y
		public int tapCount;											// Current tap count
		
		public bool IsFingerOnLeftSide { get; private set; }
		
		private int lastFingerId = -1;								// Finger last used for this joystick
		private float tapTimeWindow;							// How much time there is left for a tap to occur
		private Vector2 fingerDownPos;
		private float fingerDownTime;
		private float firstDeltaTime = 0.5f;

		public Dictionary<int, TouchInfo> Touches { get; private set; }

		// Use this for initialization
		void Start ()
		{
				Touches = new Dictionary<int, TouchInfo> ();
				IsFingerOnLeftSide = false;
				transform.position = new Vector3 (0f, 0f, transform.position.z);
				touchZone = new Rect (0f, 0f, Screen.width, Screen.height);
		}

		public void Disable ()
		{
				gameObject.SetActive (false);
				enumeratedJoysticks = false;
		}
	
		void ResetJoystick ()
		{
				// Release the finger control and set the joystick back to the default position
				lastFingerId = -1;
				foreach (var item in Touches) {
						item.Value.Position = Vector2.Lerp (position, Vector2.zero, 0.1f);
				}
//				position = Vector2.Lerp (position, Vector2.zero, 0.1f);
				Debug.Log ("reset joystick");
				fingerDownPos = Vector2.zero;
		}
	
		public bool IsFingerDown ()
		{
				return (lastFingerId != -1);
		}
	
		void LatchedFinger (int fingerId)
		{
				// If another joystick has latched this finger, then we must release it
				if (lastFingerId == fingerId) {
						ResetJoystick ();
				}
		}

		bool IsLeftSide (Vector2 touchPos)
		{
				return touchPos.x < Screen.width / 2f;
		}

		Vector2 GetTouchPosAdjusted (Touch touch)
		{
				var xPos = Mathf.Clamp ((touch.position.x - fingerDownPos.x) / (touchZone.width / 2), -1, 1);
				var yPos = Mathf.Clamp ((touch.position.y - fingerDownPos.y) / (touchZone.height / 2), -1, 1);
				var position = new Vector2 (xPos, yPos);
				return position;
		}
	
		void Update ()
		{	
				if (!enumeratedJoysticks) {
						// Collect all joysticks in the game, so we can relay finger latching messages
						joysticks = FindObjectsOfType<JoystickNew> () as JoystickNew[];
						enumeratedJoysticks = true;
				}	
		
				var count = Input.touchCount;
		
				// Adjust the tap time window while it still available
				if (tapTimeWindow > 0) {
						tapTimeWindow -= Time.deltaTime;
				} else {
						tapCount = 0;
				}
				if (count == 0) {
						ResetJoystick ();
				} else {
						for (int i = 0; i < count; i++) {
								Touch touch = Input.GetTouch (i);
								var fingerId = touch.fingerId;
								Vector2 touchPos = touch.position;

								if (!Touches.ContainsKey (fingerId)) {
										Touches.Add (fingerId, new TouchInfo () {
											Id = fingerId,
											Touch = touch,
											Position = GetTouchPosAdjusted(touch),
											IsOnLeftSide = IsLeftSide(touchPos)// touchPos.x < Screen.width / 2f
										});
								} else {

										Touches [fingerId].Position = GetTouchPosAdjusted (touch);
										Touches [fingerId].IsOnLeftSide = IsLeftSide (touchPos);
								}
								
						
								IsFingerOnLeftSide = touchPos.x < Screen.width / 2f;
						
								var shouldLatchFinger = false;
								if (touchZone.Contains (touch.position)) {
										shouldLatchFinger = true;
								}
//						Debug.Log ("shouldLatchFinger: " + shouldLatchFinger);

								// Latch the finger if this is a new touch
								if (shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId)) {
					
										lastFingerId = touch.fingerId;
										fingerDownPos = touch.position;
										fingerDownTime = Time.time;
					
//										// Accumulate taps if it is within the time window
//										if (tapTimeWindow > 0) {
//												tapCount++;
//										} else {
//												tapCount = 1;
//												tapTimeWindow = tapTimeDelta;
//										}
					
										// Tell other joysticks we've latched this finger
										foreach (JoystickNew j in joysticks) {
												if (j != this) {
														j.LatchedFinger (touch.fingerId);
												}
										}						
								}				
				
								if (lastFingerId == touch.fingerId) {	
										// Override the tap count with what the iPhone SDK reports if it is greater
										// This is a workaround, since the iPhone SDK does not currently track taps
										// for multiple touches
										if (touch.tapCount > tapCount) {
												tapCount = touch.tapCount;
										}
										// For a touchpad, let's just set the position directly based on distance from initial touchdown
										var xPos = Mathf.Clamp ((touch.position.x - fingerDownPos.x) / (touchZone.width / 2), -1, 1);
										var yPos = Mathf.Clamp ((touch.position.y - fingerDownPos.y) / (touchZone.height / 2), -1, 1);
										var position = new Vector2 (xPos, yPos);

//										Touches [i].Position = position;


										//								Debug.Log (touch.position.x + "/" + touchZone.width);
					
										if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
//												ResetJoystick ();
//												Touches [fingerId].Position = Vector2.zero;
												if (Touches.ContainsKey (fingerId)) {
														Touches.Remove (fingerId);
												}
										}
								}			
						}
						if (Touches.Count == 0) {
								ResetJoystick ();
						}
//						Debug.Log (Touches.Count);
				}

//				// Adjust for dead zone	
//				var absoluteX = Mathf.Abs (position.x);
//				var absoluteY = Mathf.Abs (position.y);
//		
//				if (absoluteX < deadZone.x) {
//						// Report the joystick as being at the center if it is within the dead zone
//						position.x = 0;
//				} else if (normalize) {
//						// Rescale the output after taking the dead zone into account
//						position.x = Mathf.Sign (position.x) * (absoluteX - deadZone.x) / (1 - deadZone.x);
//				}
//		
//				if (absoluteY < deadZone.y) {
//						// Report the joystick as being at the center if it is within the dead zone
//						position.y = 0;
//				} else if (normalize) {
//						// Rescale the output after taking the dead zone into account
//						position.y = Mathf.Sign (position.y) * (absoluteY - deadZone.y) / (1 - deadZone.y);
//				}
		}
}