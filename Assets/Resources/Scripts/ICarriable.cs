using UnityEngine;
using System.Collections;

public interface ICarriable {
	
	void Grab();
	
	void Release();
	
	void SetOwner(Transform _player);
	
}
