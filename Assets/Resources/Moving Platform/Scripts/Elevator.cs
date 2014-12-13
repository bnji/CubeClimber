using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NetworkView))]
public class Elevator : MonoBehaviour
{
	
		public AudioClip audioMoving;
	
		public enum Direction
		{
				X,
				Y,
				Z
		}
	
		public float chg = 10.0f;
		public float max = 0.0f;
		public float min = 0.0f;
		public float speed = 5.0f;
		public Direction direction = Direction.Y;
		public Transform fromObj;
		public Transform toObj;
		public bool isActive = true;
		public bool stopAtDestination = true;
		/// <summary>
		/// The wait time end station.
		/// </summary>
		public float waitTimeEndStation;
	
		public bool IsActive { get { return isActive; } set { isActive = value; } }
		public bool IsMoving { get; private set; }
	
		private int dir = 1;	
		private float posX, posY, posZ;
		private float lastTime;
	
		// Use this for initialization
		void Start ()
		{
		
				if (fromObj != null) {
						//transform.position = new Vector3(transform.position.x, fromObj.position.y - transform.localScale.y + fromObj.localScale.y, transform.position.z);
						//transform.position = fromObj.position;
				}
		
				//posX = transform.position.x;
				//posY = transform.position.y;
				//posZ = transform.position.z;
				/*if(posY < min || posY > max) {
			transform.position = new Vector3(posX, min + 0.1f, posZ);
		}*/
				//if(posY < min) posY = min + 0.1f;
				//if(posY > max) posY = max - 0.1f;
				lastTime = Time.time;
		
				audio.clip = audioMoving;
				IsMoving = false;
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (IsMoving && IsActive) {
						if (!audio.isPlaying) {
								audio.Play ();
						}
				} else {
						if (audio.isPlaying)
								audio.Stop ();
				}
		}
	
		void FixedUpdate ()
		{
		
//				if (Time.time - lastTime > waitTimeEndStation && !stopAtDestination) {
//						isActive = true;
//				}
		
				if (!isActive)
						return;
		
				if (fromObj == null && toObj == null) {
			
						// Change direction
						//print(transform.position.x + "/" +posX + ", " + transform.position.y + "/" + posY + ", " + transform.position.z + "/" + posZ);
			
						if (max != 0.0f && min != 0.0f) {
								switch (direction) {
								case Direction.X:
										if (transform.position.x + posX > max + posX || transform.position.x + posX < min + posX) {
												ChangeDirection ();
										}
										MovePlatform (transform, Vector3.left);
										break;
								case Direction.Y:
										if (transform.position.y + posY >= max + posY || transform.position.y + posY <= min + posY) {
												ChangeDirection ();
										}
						//transform.Translate(new Vector3(posX, posY, posZ) * dir * Time.deltaTime * 0.1f);
										MovePlatform (transform, Vector3.up);
										break;
								case Direction.Z:
										if (transform.position.z + posZ > max + posZ || transform.position.z + posZ < min + posZ) {
												ChangeDirection ();
										}
										MovePlatform (transform, Vector3.forward);
										break;
								}
						} else {
								switch (direction) {
								case Direction.X:
										if (transform.position.x > posX + chg || transform.position.x < posX - chg) {
												ChangeDirection ();
										}
										MovePlatform (transform, Vector3.left);
										break;
								case Direction.Y:
										if (transform.position.y > posY + chg || transform.position.y < posY - chg) {
												ChangeDirection ();
										}
										MovePlatform (transform, Vector3.up);
										break;
								case Direction.Z:
										if (transform.position.z > posZ + chg || transform.position.z < posZ - chg) {
												ChangeDirection ();
										}
										MovePlatform (transform, Vector3.forward);
										break;
								}
						}
			
				} else {
						//float i = Mathf.PingPong(Time.time * speed * Time.deltaTime, 1);
						//transform.position = Vector3.Lerp(fromObj.position, toObj.position, i);
						float newValue = float.MaxValue;
						Vector3 distance = Vector3.zero;
						if (dir == 1) {
								newValue = (transform.position - toObj.position).magnitude;
								distance = (toObj.position - fromObj.position).normalized;
						} else { // dir == -1
								newValue = (transform.position - fromObj.position).magnitude;
								distance = (fromObj.position - toObj.position).normalized;
						}
						if (newValue < smallestValue) {
								smallestValue = newValue;
						} else {
								Debug.Log (isActive);
								ChangeDirection ();
								Debug.Log (isActive);
								smallestValue = float.MaxValue;
						}
						transform.Translate (distance * Time.deltaTime * speed);
						IsMoving = true;
				}
		}
	
		private float smallestValue = float.MaxValue;
	
		void ChangeDirection ()
		{
				IsMoving = false;
				if (audio.isPlaying)
						audio.Stop ();
				if (waitTimeEndStation > 0) {
						lastTime = Time.time;
						isActive = false;
				}
				if (stopAtDestination) {
						isActive = false;
				}
				dir = dir * -1;
		}
	
		void MovePlatform (Transform t, Vector3 v)
		{
				t.Translate (v * dir * speed * Time.deltaTime, Space.Self);
				IsMoving = true;
		}
}