﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	
	
	private Transform[] weaponInventory = new Transform[2];
	
	private bool gameStarted = false;
	private int selected = 0;
	
	public void SetWeaponSelection(Transform primary, Transform secondary)
	{
		if (weaponInventory[0] != null) Destroy(weaponInventory[0]);
		if (weaponInventory[1] != null) Destroy(weaponInventory[1]);
		weaponInventory[0] = Instantiate(primary, transform.position, transform.rotation) as Transform;
		weaponInventory[1] = Instantiate(secondary, transform.position, transform.rotation) as Transform;
		weaponInventory[0].parent = transform;
		weaponInventory[1].parent = transform;
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetButton("Fire1") && !GameManager.Instance.GameIsPaused)
		{
			weaponInventory[selected].BroadcastMessage("Fire");
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			if (selected == 0) selected = 1;
			else selected = 0;
		}
		
	}
	
	void OnGUI()
	{
	
	}
}