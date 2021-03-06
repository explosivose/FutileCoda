﻿using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour 
{
	public bool debug = false;
	public AudioClip firingSound;		// sound to play when shooting
	public float rateOfFire = 2f;		// how many times to fire in one second
	public float bulletSpread = 8f; 	// size of spread cone (degrees)
	public float gunNozzleSize = 0.15f; // size of nozzle
	public Transform projectile;		// the projectile to shoot!
	public float projectileSpeed;		// how fast the projectile will travel initially
	private Transform gunman;			// refer to the player for stuff
	private bool firing = false;		// whether or not we're currently firing the gun
	
	public void SetGunman(Transform newgunman)
	{
		gunman = newgunman;
	}
	
	void Awake()
	{
		gunman = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (debug)
		{
			if (Input.GetButton("Fire1"))
			{
				Fire ();
			}
		}
	}
	
	void Fire()
	{
		if (!firing) StartCoroutine(SingleShot());
	}
	
	IEnumerator SingleShot()
	{
		firing = true;
		
		AudioSource.PlayClipAtPoint(firingSound, gunman.position);
		
		// setup bullet spread
		float angle = 8f;
		float angleSpread = Mathf.Deg2Rad *  Mathf.Clamp(90f-angle,Mathf.Epsilon,90f-Mathf.Epsilon) ;
		float distance = Mathf.Tan(angleSpread);
		
		Vector3 nozzle = gunman.position + gunman.forward;
		Debug.DrawRay(gunman.position, gunman.forward,Color.red, 5f);
		for (int i = 0; i < 8; i++)
		{
			Vector2 pointInCircle = Random.insideUnitCircle;
			Vector3 bulletDirection = new Vector3(pointInCircle.x, pointInCircle.y, distance).normalized;
			if (gunman.tag == "Player")
			{
				bulletDirection = Camera.main.transform.rotation * bulletDirection;
			}
			else
			{
				bulletDirection = gunman.rotation * bulletDirection;
			}
			
			Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection);
			Vector3 bulletSpawn = nozzle + (Random.insideUnitSphere * gunNozzleSize);
			Transform b = Instantiate(projectile, bulletSpawn, bulletRotation) as Transform;
			b.rigidbody.AddForce(bulletDirection * projectileSpeed, ForceMode.VelocityChange);
			
			if (gunman.tag == "Player")
				b.BroadcastMessage("SetDamageSource", Projectile.Source.Player);
			else
				b.BroadcastMessage("SetDamageSource", Projectile.Source.Enemy);
		}
		
		if (gunman.tag =="Player")
			ScreenShake.Instance.Shake(0.5f, 2f);
		
		yield return new WaitForSeconds(1/rateOfFire);
		
		firing = false;
	}
}
