﻿using UnityEngine;
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
	
	public void SetCamera(Transform newCamera)
	{
		cam = newCamera;
		originalPosition = newCamera.position;
	}
	
	void Update()
	{
		if (GameManager.Instance.GameIsPaused) return;
		if (cam == null) return;
		if (mag > 0f)
		{
			cam.localPosition = originalPosition + Random.insideUnitSphere * mag;
			mag -= t * Time.deltaTime;
		}
		else 
		{
			cam.localPosition = originalPosition;
		}
	}
}
