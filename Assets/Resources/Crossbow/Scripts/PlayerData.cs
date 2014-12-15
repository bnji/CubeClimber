using UnityEngine;
using System.Collections;
using System;

//[Serializable]
//public class PlayerData { // : ScriptableObject {
//	
//	public string Name { get; set; }
//	
//	public float Health { get; set; }
//	
//	public PlayerData() { }
//}

[System.Serializable]
public class PlayerData
{
		public string Name { get; set; }

		[SerializeField]
		private bool
				_isInvincible;
		public bool IsInvincible {
				get { return _isInvincible; }
				set { _isInvincible = value; }
		}
		[SerializeField]
		private float
				_id;
		public float ID {
				get { return _id; }
				set { _id = value; }
		}
		[SerializeField]
		private float
				_score;
		public float Score {
				get { return _score; }
				set {
						_score = value;
						PlayerPrefs.SetFloat (_id + "_score", value);
				}
		}
		[SerializeField]
		private float
				_health;
		public float Health {
				get { return _health; }
				set {
						_health = value; 
						PlayerPrefs.SetFloat (_id + "_health", value);
				}
		}
		[SerializeField]
		private float
				_maxHealth;
		public float MaxHealth {
				get { return _maxHealth; }
				set {
						_maxHealth = value; 
						PlayerPrefs.SetFloat (_id + "_maxHealth", value);
				}
		}
		[SerializeField]
		private float
				_lethalVelocity;
		public float LethalVelocity {
				get { return _lethalVelocity; }
				set {
						_lethalVelocity = value; 
						PlayerPrefs.SetFloat (_id + "_lethalVelocity", value);
				}
		}
		[SerializeField]
		private ControllerInfo
				_controllerInfo;
		public ControllerInfo ControllerInfo {
				get { return _controllerInfo; }
				set {
						_controllerInfo = value; 
						//PlayerPrefs.SetFloat (_id + "_controllerInfo", value);
				}
		}
	
		public void Load ()
		{
				_score = PlayerPrefs.GetFloat (_id + "_score");
				_health = PlayerPrefs.GetFloat (_id + "_health");
				_maxHealth = PlayerPrefs.GetFloat (_id + "_maxHealth");
				_lethalVelocity = PlayerPrefs.GetFloat (_id + "_lethalVelocity");
				//_controllerInfo = PlayerPrefs.GetFloat (_id + "_controllerInfo");
		}
	
		public void Reset ()
		{
		
		}
}