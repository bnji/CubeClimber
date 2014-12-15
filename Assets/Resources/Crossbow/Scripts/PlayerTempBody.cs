using UnityEngine;
using System.Collections;


public class PlayerTempBody : MonoBehaviour {
	
	Rigidbody _playerTempRigidbody;
	
	void Awake() {
		if(_playerTempRigidbody == null) {
			_playerTempRigidbody = GetComponent<PlayerExtScr>().gameObject.AddComponent<Rigidbody>();
			_playerTempRigidbody.constraints = RigidbodyConstraints.None;
			//_playerTempRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			_playerTempRigidbody.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
			_playerTempRigidbody.useGravity = true;
			_playerTempRigidbody.isKinematic = false;
			//_playerTempRigidbody.mass = 1f;
		}
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision collision) {
		if(_playerTempRigidbody == null) return;
		
		if(!transform.parent.name.Equals(collision.collider.name)) {
			gameObject.SendMessage("OnLand", collision, SendMessageOptions.DontRequireReceiver);
		}
	}
}
