using UnityEngine;
using System.Collections;

public class WeatherController : MonoSingleton<WeatherController> {

	public void UseFog(bool v) {
		RenderSettings.fog = v;
	}
}
