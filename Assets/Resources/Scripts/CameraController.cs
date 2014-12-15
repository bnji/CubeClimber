using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CameraController : MonoSingleton<CameraController>
{
		private List<Camera> cameraList = new List<Camera> ();
		public Camera mainCamera;
		public bool canActivateAudioListener = false;
		public Camera ActiveCamera { get { return activeCamera; } set { activeCamera = value; } }
		private Camera activeCamera;// = null;
		
//		private Rect tempNormalizedViewPortRect;
		private float oldFOV;
	
		public bool isActive;
	
		// Use this for initialization
		void Start ()
		{
				// Get the camera of the player
				Camera _mainCamera = gameObject.GetComponentInChildren<Camera> ();
		
				// Make sure we have a main camera
				if (mainCamera == null)
						mainCamera = _mainCamera;

				//SetViewPortSize (20000.0f, 10.0f / 16.0f, 0.05f);
				List<Camera> _cameraList = new List<Camera> (GameObject.FindObjectsOfType (typeof(Camera)) as Camera[]);
				cameraList = new List<Camera> ();
				_cameraList.ForEach ((Camera camera) => {
						if (!camera.name.Equals (mainCamera.name)) {
								camera.depth = -1; // Make sure the camera is under the main camera ('disable' it)
								if (camera.audio != null)
										camera.audio.enabled = false; // Disable audio listener
//								camera.rect = tempNormalizedViewPortRect;
						}
						cameraList.Add (camera);
				});
				_cameraList.Clear ();
				activeCamera = mainCamera;
				
		}
	
//		void SetViewPortSize (float _size, float _ratio, float _modifier)
//		{
//				float width = _size;
//				float height = _ratio * _size;
//				float x = width - _modifier * _ratio;
//				float y = height - _modifier * _ratio;
//				tempNormalizedViewPortRect = new Rect (-x, -y, width, height);
//		}
	
//		// Update is called once per frame
//		void Update ()
//		{
//				// ESCAPE key should exit any camera
//				if (InputController.instance.Process (KeyCode.Escape)) {				
//						SetMainCameraActive ();
//						//InputController.instance.SetPlayerActive(true);
//				}
//		}
	
		/// <summary>
		/// Gets the camera.
		/// </summary>
		/// <returns>
		/// The camera.
		/// </returns>
		/// <param name='_cameraName'>
		/// _camera name.
		/// </param>
		public Camera GetCamera (string _cameraName)
		{
				return cameraList.Find ((Camera ct) => ct.camera.name == _cameraName).camera;
		}
	
		public void ChangeCamera (string cameraName)
		{
				Debug.Log ("change camera");
				// Find the camera
				Camera newCamera = GetCamera (cameraName);
				if (activeCamera == null)
						activeCamera = mainCamera;
//				bool isMainCamera = newCamera.name.Equals (mainCamera.name);
//				bool isFoundCameraCurrentlyActive = newCamera.name.Equals (activeCamera.name);
		
				// Do changes to the camera we are changing from...
				activeCamera.depth = -1;
				// Set the camera back to taking up the full screen
//				activeCamera.rect = tempNormalizedViewPortRect;
				if (activeCamera.audio != null)
						activeCamera.audio.enabled = false;
				
				// Set the found camera as the active camera
				if (newCamera.name.Equals (activeCamera.name)) {
						activeCamera = mainCamera;
						activeCamera.depth = 0;
						// Activate the player controls
						//InputController.instance.SetPlayerActive(true);
				} else {
						activeCamera = newCamera;
						activeCamera.depth = 1;
						// Deactivate the player controls
						Debug.Log ("Deactivated player");
//						InputController.instance.SetPlayerActive (false);
				}
		
				// Set the camera back to taking up the full screen
				activeCamera.rect = new Rect (0, 0, 1, 1);
				// Do changes to the camera we have changed to...
				if (activeCamera.audio != null)
						activeCamera.audio.enabled = true;
		

		}
		
}