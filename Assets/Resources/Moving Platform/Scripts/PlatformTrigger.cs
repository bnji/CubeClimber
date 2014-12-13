using UnityEngine;
using System.Collections;

public class PlatformTrigger : MonoBehaviour {
	
	public Elevator platform;
	public bool isTriggerActive;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerStay(Collider other) {
		if(isTriggerActive)
			platform.IsActive = true;
	}
}
