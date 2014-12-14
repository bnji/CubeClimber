using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public abstract class AstronomicalObject : MonoBehaviour, IPause
{
		private float initialMass;
		public bool HasPlayer { get; set; }
		public AudioClip audioImpactExplosion;
		public float movementSpeed = 0.0f;
		public bool isStationary = false;
		public bool isRotating = false;
		public bool isIndestructible = false;
		public PlanetMovementDirection movementDirection = PlanetMovementDirection.COUNTER_CLOCKWISE;
		public bool useAudioInSpace = false;
		public float lifeSpan = 0f;

		protected float lifeStarted;
		protected int rotDir = 1;
	

		protected bool isPaused = false;
	
		public void OnPauseGame ()
		{
				isPaused = true;
		}
	
		public void OnResumeGame (bool hideCursor)
		{
				isPaused = false;
		}

		public void ResetMass ()
		{
				rigidbody.mass = initialMass;
		}

		public void IncrementMass (float amount)
		{
				rigidbody.mass += amount;
		}
	
		void Awake ()
		{
		}
	
		// Use this for initialization
		void Start ()
		{
				initialMass = rigidbody.mass;
				rotDir = movementDirection == PlanetMovementDirection.COUNTER_CLOCKWISE ? 1 : -1;
				lifeStarted = Time.time;
		}
	
		// Update is called once per frame
		protected virtual void FixedUpdate ()
		{
				if (isPaused) {
						return;
				}
//				Debug.Log (Time.time + " / " + lifeStarted + " " + lifeSpan);
				if (lifeSpan != 0 && Time.time - lifeStarted > lifeSpan) {
						//StartCoroutine (PlayExplosion ());
						Destroy (gameObject);
				}
		}
	
		protected IEnumerator PlayExplosion ()
		{
				if (audioImpactExplosion == null)
						yield return new WaitForEndOfFrame ();
				else {
						AudioSource.PlayClipAtPoint (audioImpactExplosion, transform.position);
						yield return new WaitForSeconds (audioImpactExplosion.length);
				}
		}
}

public enum PlanetMovementDirection
{
		CLOCKWISE,
		COUNTER_CLOCKWISE
}