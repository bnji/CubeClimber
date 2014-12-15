using UnityEngine;
using System.Collections;

public class FPSCounter : MonoSingleton<FPSCounter> {

	// FPS
	private float updateInterval = 0.5f;
	private float accum = 0.0f; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	private string fps = "";
	
	public string FPS { get { return fps; } }
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateFPS();
	}
	
	void UpdateFPS() {
		timeleft -= Time.deltaTime;
	    accum += Time.timeScale/Time.deltaTime;
	    ++frames;	 
	    // Interval ended - update GUI text and start new interval
	    if( timeleft <= 0.0 )
	    {
	        // display two fractional digits (f2 format)
			fps = (accum/frames).ToString("f2");
	        timeleft = updateInterval;
	        accum = 0.0f;
	        frames = 0;
	    }
	}
	
	
}
