using UnityEngine;
using System.Collections;

public class StationaryTrigger : MonoBehaviour {
	
	public bool isTriggerActive;
	public float startAfter;
	
	private Elevator platform;
	private float timer;
	private bool isStarted;
	
	// Use this for initialization
	void Start () {
		platform = transform.parent.GetComponent<Elevator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - timer > startAfter && isStarted) {
			isStarted = false;
			StartPlatform();
		}
	}
	
	void OnTriggerEnter(Collider other) {
		isStarted = true;
		timer = Time.time;
	}
	
	void OnTriggerExit(Collider other) {		
		//if(isTriggerActive && platform.isActive)
			//platform.IsActive = false;
	}
	
	void StartPlatform() {
		if(isTriggerActive && !platform.isActive)
			platform.IsActive = true;
	}
}
