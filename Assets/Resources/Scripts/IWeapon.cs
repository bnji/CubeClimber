using UnityEngine;
using System.Collections;

public interface IWeapon {

	int Ammo { get; set; }
	
	bool CanShoot { get; set; }
	
	void Fire1(Vector3 position, Transform owner, GameObject target);
	
	void Fire2();
	
	void Reload();
	
	void SetRelaxedMode();
	
	void SetWeaponArmedMode();
	
	void SetAimingMode();
}
