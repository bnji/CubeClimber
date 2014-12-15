using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour, IProjectile
{
	
		public enum JointType
		{
				Ball,
				Hinge 
		}
		
		public JointType jointType;
	
		private Vector3 startPos;
		private bool hasCollided = false;
		private bool isHooked = false;
		private IGrabbable grabbableScript;
		private float damage = 0.0f;
		public float speed = 1.0f;
		public float maxSpeed = 30.0f;
		public float speedIncrement = 0.2f;
		private Vector3 endPosition;
		private Transform owner;
		private GameObject target;
		public bool IsHooked { get { return isHooked; } }
		public Vector3 RopeStart { get; private set; }
		public Vector3 RopeEnd { get; private set; }
		private LineRenderer lineRenderer;
		private Camera arrowCamera = null;
		private Camera playerCamera = null;
		private bool changeToCameraOnStart = false;
		private Rigidbody weaponHolderRigidBody;
		private bool hasReachedZero = false;
	
		public void Initialize (bool _changeToCameraOnStart, Rigidbody _weaponHolderRigidBody, Vector3 _startPos, Transform _owner, GameObject _target)
		{
				changeToCameraOnStart = _changeToCameraOnStart;
				weaponHolderRigidBody = _weaponHolderRigidBody;
				owner = _owner;
				target = _target;
				startPos = _startPos;
				endPosition = _target.transform.position;
		}
	
		// Use this for initialization
		void Start ()
		{
				lineRenderer = gameObject.AddComponent<LineRenderer> ();
				lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));
				lineRenderer.SetColors (Color.gray, Color.gray);
				lineRenderer.SetWidth (0.05f, 0.05f);
		
				isHooked = false;
				arrowCamera = GetComponentInChildren<Camera> ();
				arrowCamera.enabled = false;
				playerCamera = Camera.main;
				if (changeToCameraOnStart) {
						playerCamera.enabled = false;
						arrowCamera.enabled = true;
				}
				transform.rotation = owner.rotation;
		}
	
		// Update is called once per frame
		void Update ()
		{
				MoveArrow ();
		}
	
		void MoveArrow ()
		{
				if (!hasCollided) {
						//print(transform.position + " " + endPosition);
						//print ((endPosition - transform.position).magnitude);
						if (!hasReachedZero) {
								Time.timeScale -= 0.1f;
								if (changeToCameraOnStart) {
										arrowCamera.fieldOfView += 0.3f;
								}
						}
						if (Time.timeScale <= 0.5f) {
								hasReachedZero = true;	
						}
						if (hasReachedZero && Time.timeScale < 3.0f) {
								if (changeToCameraOnStart) {
										arrowCamera.fieldOfView -= 0.3f;
								}
								Time.timeScale += 0.3f;
						}
						if (speed <= maxSpeed) {
								speed += speedIncrement;
						}
			
						// Solution to lerp problem: http://forum.unity3d.com/threads/89998-Vector3-lerp-problem
						transform.Translate ((endPosition - startPos).normalized * speed * Time.deltaTime, Space.World);
						//transform.position = Vector3.Lerp(startPos, endPosition, speed * Time.deltaTime);
						if (arrowCamera != null && changeToCameraOnStart) {
								arrowCamera.transform.LookAt (endPosition);
								arrowCamera.transform.position = transform.position;
						}
			
						if (lineRenderer != null) {
								lineRenderer.SetPosition (0, RopeStart + new Vector3 (0f, 0f, 1f));
								lineRenderer.SetPosition (1, RopeEnd);
						}
				}
		}
	
		public void SetHingeJointRigidBody (Rigidbody body)
		{
				//gameObject.GetComponent<HingeJoint>().hingeJoint.connectedBody = body;
				gameObject.GetComponent<Joint> ().connectedBody = body;
		}
	
		void OnCollisionEnter (Collision collision)
		{
				if (collision.collider.tag.ToLower ().Equals ("grabbable_object")) { //if(typeof(IGrabbable).IsAssignableFrom(collision.collider.GetType())) {
						if (!hasCollided) {
//								print (transform.name + " collided with " + collision.collider.name);
								Time.timeScale = 1.0f;
								rigidbody.constraints = RigidbodyConstraints.FreezeAll; // freeze the arrow in place
			
								if (jointType == JointType.Ball)
										gameObject.AddComponent<CharacterJoint> ();
								else if (jointType == JointType.Hinge)
										gameObject.AddComponent<HingeJoint> ();
			
								Joint joint = gameObject.GetComponent<Joint> ();
								joint.connectedBody = weaponHolderRigidBody;
								ChangeCamera ();
								ActivateHook ();
								hasCollided = true;
						}
				} else {
//						Destroy ();
						//print ("arrow collided with: " + collision.collider.name);
				}
		}
	
		void ActivateHook ()
		{
				if (!isHooked) {
						grabbableScript = (IGrabbable)target.GetComponent (typeof(IGrabbable));
						if (grabbableScript != null) {
//								print ("is grabbable");
								rigidbody.constraints = RigidbodyConstraints.FreezeAll;
								grabbableScript.Grab ();
						} else {
								Destroy (transform.gameObject);
						}
				}
				isHooked = true;
		}
	
		public void SetRopePoints (Vector3 start, Vector3 end)
		{
				RopeStart = start;
				RopeEnd = end;
				Debug.DrawLine (RopeStart, RopeEnd, Color.red);
				lineRenderer.SetPosition (0, RopeStart);
				lineRenderer.SetPosition (1, RopeEnd);
		}
	
		void ChangeCamera ()
		{
				if (arrowCamera != null && changeToCameraOnStart) {
						arrowCamera.enabled = false;
						playerCamera.enabled = true;
						arrowCamera = null;
				}
			
		}
	
		public void Destroy ()
		{
				ChangeCamera ();
				Destroy (transform.gameObject);
		}
	
	#region IProjectile implementation
		public float Damage {
				get {
						return damage;
				}
				set {
						damage = value;
				}
		}

		public float Speed {
				get {
						return speed;
				}
				set {
						speed = value;
				}
		}
	
	#endregion
}
