using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlanetScript : AstronomicalObject, IDestroyable
{
		public bool canLandOnPlanet = true;
		private List<PlanetScript> planets;

		void Awake ()
		{
				FindPlanets ();
		}

		// Update is called once per frame
		protected override void FixedUpdate ()
		{
				if (isPaused) {
						return;
				}

				if (planets == null)
						return;

//				Debug.Log (planets.Count);
		
				//foreach (var planet in planets) {
				for (int i = 0; i < planets.Count; i++) {
						PlanetScript planet = planets [i];
						if (planet == null)
								return;
						// if the planet (AO) is stationary, then don't apply any gravitational force to it or it's surrounding planets
						if (isStationary)// || planet.isStationary)
								return;
			
			
						//if (!this.Equals (planet)) {
						PhysicsHelper.ApplyGravity (this, planet);
						//}
						if (!isRotating) {
								Vector3 dist = rigidbody.transform.position - planet.rigidbody.transform.position; 
//								float r = dist.magnitude / 2;
//								float v = planet.rigidbody.velocity.magnitude;
								transform.RotateAround (planet.transform.position, rotDir * Vector3.forward, movementSpeed * Time.deltaTime);
						}
				}
				base.FixedUpdate ();
		}




		void OnCollisionEnter (Collision collision)
		{
				if (!canLandOnPlanet)
						return;

		
				GameObject otherGameObject = collision.gameObject;
				//Debug.Log ("landed on " + go.name);
				otherGameObject.SendMessage ("OnCollisionWithPlanet", this, SendMessageOptions.DontRequireReceiver);
				// Get the planet script component of the colliding gameobject
				PlanetScript otherPlanet = otherGameObject.GetComponent<PlanetScript> ();
				if (otherPlanet != null) {
						// Don't process if can't land on planet (for example if it's a health pickup)
						if (!otherPlanet.canLandOnPlanet)
								return;

				
						// if the object has a planet script, is destructable and a lower mass, then process it
						if (!otherPlanet.isIndestructible && otherPlanet.rigidbody.mass <= rigidbody.mass) {

				
								//Debug.Log ("on collision enter");
								//PlanetController pc = GameObject.FindObjectOfType<PlanetController> ();
								// Don't process indescrutable planets
								//if (pc != null && !pc.indestructableObjects.Contains (collision.gameObject)) {
								//Debug.Log (ps.transform.name);
				
								// Get the random color changer component
								/*RandomColorChanger rcc = go.GetComponent<RandomColorChanger> ();
								RandomColor2 rc2 = ps.GetComponent<RandomColor2> ();
								if (rcc != null && rc2 != null) {
										if (rcc.CurrentColor.Equals (rc2.PlanetColor)) {
												Debug.Log (rcc.CurrentColor + " - " + rc2.PlanetColor);
												Debug.Log ("Same color!");
												// Update the score
												GameObject.FindObjectOfType<Score> ().UpdateScore (1);
										}
								}*/

								if (name == "Player") {
										Debug.Log ("its player");
								} else {
										otherPlanet.enabled = false;
										
//										Destroy (otherPlanet);
										otherPlanet.Destroy ();
								}
								
								/*for (int i = 0; i < planets.Count; i++) {
										planets [i].planets.Remove (ps);
								}*/
								//GameObject.FindObjectOfType<Player> ().DetachFromPlanet (ps);
								//planets.Remove (ps);
								// Destroy the planet
								//Destroy (ps.gameObject);
								
								//Debug.Log (planets.Count);
								//FindPlanets ();
								//}
						}		
				}
				//Debug.Log (collision.gameObject.name + " collided with " + this.gameObject.name);
		}

		void OnCollisionStay (Collision collision)
		{
		}

		void OnObjectEnter (GameObject playerGameObject)
		{
				AddCitizen (playerGameObject);
				//Debug.Log ("player landed on " + gameObject.name);
		}

		List<PlanetCitizen> citizens = new List<PlanetCitizen> ();

		void AddCitizen (GameObject playerGameObject)
		{
				PlanetCitizen citizen = (PlanetCitizen)playerGameObject.GetComponent (typeof(PlanetCitizen));
//				Debug.Log (citizen);
				if (citizen != null) {
						if (!citizens.Contains (citizen)) {
								citizens.Add (citizen);
						}
						HasPlayer = true;
				}
//				Debug.Log ("hasPlayer: " + HasPlayer);
		}

		void OnObjectExit ()
		{
				//Debug.Log ("player left " + gameObject.name);
				// Make sure the last planet we landed on can get destroyed
				isIndestructible = false;
//				PlanetPickupSpawner planetPickupSpawner = gameObject.GetComponentInChildren<PlanetPickupSpawner> ();
//				if (planetPickupSpawner != null) {
//						planetPickupSpawner.SetCanSpawn ();
//				}
		}

		void OnFloatingInSpace (ControllerInfo controllerInfo)
		{
//				// Increase the gravity of the player while in the air?
		}
	
		void OnEnterAthmosphere (PlanetScript ps)
		{
				//Debug.Log ("Added planet " + ps.name + " to planet " + name);
				if (planets != null && !planets.Contains (ps))
						planets.Add (ps);
		}
	
		void OnExitAthmosphere (PlanetScript ps)
		{
				//Debug.Log ("Removed planet " + ps.name + " from planet " + name);
				if (planets != null && planets.Contains (ps))
						planets.Remove (ps);
		}
	
		public bool IsDestroyable { get; set; }

		public void Destroy ()
		{
//				Debug.Log (HasPlayer);
//				Debug.Log ("destroyed " + gameObject.name);
				foreach (var citizen in citizens) {
						citizen.gameObject.SendMessage ("OnPlanetDestroyed", gameObject, SendMessageOptions.DontRequireReceiver);
				}
				Destroy (this);
		}

		void Destroy (PlanetScript ps)
		{
				if (ps != null) {
						//Debug.Log ("playing explosion audio: " + ps.audioImpactExplosion);								
						if (useAudioInSpace)
								StartCoroutine (ps.PlayExplosion ());
		
						// Remove the planet from the list in the other planets
						for (int i = 0; i < planets.Count; i++) {
								if (planets [i] != null && planets [i].enabled) {
										planets [i].planets.Remove (ps);
								}
						}
						// Remove and destroy the mini planet from the dictionary
						// Destroy the planet
						Destroy (ps.gameObject);
				}
		}
	
		void FindPlanets ()
		{
				var gameHandler = GameObject.FindObjectOfType<GameHandler> ();
//				var gameHandler = GameObject.FindGameObjectWithTag ("GameHandler").GetComponent<GameHandler> ();

				// Find all the planets in the scene
				if (gameHandler != null && gameHandler.useGlobalGravity) 
						planets = new List<PlanetScript> (FindObjectsOfType<PlanetScript> ());
				else
						planets = new List<PlanetScript> ();

//				Debug.Log (planets.Count);
		}
}