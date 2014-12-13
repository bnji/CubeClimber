using UnityEngine;
using System.Collections;

public class PlatformTrigger2 : MonoBehaviour {
	
	private Elevator platform;
	public bool isTriggerActive;
	
	// Use this for initialization
	void Start () {
		platform = transform.parent.GetComponentInChildren<Elevator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}	
	
	void OnTriggerEnter(Collider other) {
		StartPlatform();
	}
	
	void StartPlatform() {
		if(isTriggerActive && !platform.isActive)
			platform.IsActive = true;
	}
}
