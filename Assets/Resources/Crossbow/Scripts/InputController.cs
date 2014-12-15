using UnityEngine;
using System.Collections.Generic;

public class InputController : MonoSingleton<InputController>
{
		private bool isMouseLocked = false;
		private CharacterController[] controllers;
		private MouseLook[] mouseLooks;
	
		public override void Init ()
		{
				GetControllers (true);
		}
	
		private Dictionary<string, bool> oldCCState;
		private Dictionary<string, bool> oldMLState;
	
		public void SetPlayerActive (bool state)
		{
				GetControllers (state);
		
				if (controllers != null) {
						for (int i = 0; i < controllers.Length; i++) {
								CharacterController cc = controllers [i];
								if (cc != null) {
										if (state == false) { //disabled state
												cc.enabled = state;
										} else { // old state (enabled or disabled state)
												cc.enabled = oldCCState [cc.name];
										}
								}
						}
				}
				if (mouseLooks != null) {
						for (int i = 0; i < mouseLooks.Length; i++) {
								MouseLook ml = mouseLooks [i];
								if (ml != null) {
										if (state == false) { //disabled state
												ml.enabled = state;
										} else { // old state (enabled or disabled state)
												ml.enabled = oldMLState [ml.name];
										}
								}
						}
				}
		}
	
		private void GetControllers (bool state)
		{
				// Get the players character motor
				controllers = GameObject.FindObjectsOfType (typeof(CharacterController)) as CharacterController[];
				// only get controllers old state, when
				if (state == false) {
						oldCCState = new Dictionary<string, bool> ();
						for (int i = 0; i < controllers.Length; i++) {
								oldCCState.Add (controllers [i].name, controllers [i].enabled);
						}
				}
				// Get the players mouse look
				mouseLooks = GameObject.FindObjectsOfType (typeof(MouseLook)) as MouseLook[];
				// only get controllers old state, when
				if (state == false) {
						oldMLState = new Dictionary<string, bool> ();
						for (int i = 0; i < mouseLooks.Length; i++) {
								oldMLState.Add (mouseLooks [i].name, mouseLooks [i].enabled);
						}
				}
		}
}