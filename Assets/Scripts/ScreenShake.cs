using UnityEngine;
using System.Collections;

public class ScreenShake : Singleton<ScreenShake> 
{
	
	private Transform cam;
	private Vector3 originalPosition = Vector3.zero;
	
	private float mag;
	private float t;
	
	void Awake()
	{
		cam = Camera.main.transform;
		if (cam == null) Debug.LogError("could not find main camera");
		originalPosition = cam.localPosition;
	}
	
	public void Shake(float magnitude, float decay)
	{
		mag = magnitude;
		t = decay;
	}
	
	void OnLevelWasLoaded()
	{
		cam = Camera.main.transform;
		if (cam == null) Debug.LogError("could not find main camera");
		originalPosition = cam.localPosition;
	}
	
	void Update()
	{
		if (mag > 0f)
		{
			cam.localPosition = originalPosition + Random.insideUnitSphere * mag;
			mag -= t * Time.deltaTime;
		}
		else cam.localScale = originalPosition;
	}
}
