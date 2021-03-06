﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public int maxHealth = 100;
	
	public bool Dead
	{
		get { return isDead; }
	}
	
	public Transform[] weaponInventory = new Transform[2];
	public int selected = 0;
	public AudioClip[] hurtSound;
	private int health;
	private bool isDead = false;
	private bool hurting = false;
	private int killCount = 0;
	private GUIText healthText;
	private GUIText scoreText;
	
	public void SetWeaponSelection(Transform primary, Transform secondary)
	{
		if (weaponInventory[0] != null) Destroy(weaponInventory[0]);
		if (weaponInventory[1] != null) Destroy(weaponInventory[1]);
		weaponInventory[0] = Instantiate(primary, transform.position, transform.rotation) as Transform;
		weaponInventory[1] = Instantiate(secondary, transform.position, transform.rotation) as Transform;
		weaponInventory[0].parent = transform;
		weaponInventory[1].parent = transform;
	}
	
	public void AddKill()
	{
		killCount++;
	}
		
	
	// Use this for initialization
	void Start () 
	{
		health = maxHealth;
		healthText = GameObject.Find("HUD Text").transform.FindChild("health_value").guiText;
		scoreText = GameObject.Find ("HUD Text").transform.FindChild("score_value").guiText;
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Bullet")
		{
			if (col.gameObject.GetComponent<BasicBullet>().ProjectileSource == Projectile.Source.Enemy )
			{
				if ( col.relativeVelocity.magnitude > 10f )
				{
					Hurt (2);
				}
			}
		}
	}
	
	void Hurt()
	{
		if (isDead) return;
		health-=10;
		StartCoroutine(HurtEffect());
		if ( health <= 0 ) StartCoroutine(Die() );
	}
	
	void Hurt(int dmg)
	{
		if (isDead) return;
		health-=dmg;
		StartCoroutine(HurtEffect());
		if (health <= 0 ) StartCoroutine( Die () );
	}
	
	IEnumerator HurtEffect()
	{
		if (!isDead)
		{
			int k = Random.Range(0, hurtSound.Length);
			AudioSource.PlayClipAtPoint(hurtSound[k], transform.position);
			ScreenShake.Instance.Shake(0.25f, 1f);
			Color hurtColor = Color.red;
			hurtColor.a = 1 - (health/maxHealth);
			ScreenFade.Instance.StartFade(hurtColor,0f);
			yield return new WaitForSeconds(0.1f);
			ScreenFade.Instance.StartFade(Color.clear,0f);
			hurting = false;
		}
		else
		{
			hurting = true;
			yield return new WaitForFixedUpdate();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		healthText.text = health.ToString();
		scoreText.text = killCount.ToString();
		if (isDead) return;
		if (Input.GetButton("Fire1") && !GameManager.Instance.GameIsPaused)
		{
			weaponInventory[selected].BroadcastMessage("Fire");
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			if (selected == 0) 
			{
				selected = 1;
				GunTexture.Instance.ChangeTexture(weaponInventory[selected]);
			}	
			else 
			{
				selected = 0;
				GunTexture.Instance.ChangeTexture(weaponInventory[selected]);
			}
		}

	}
	
	
	
	IEnumerator Die()
	{
		isDead = true;
		health = 0;
		AudioListener.volume = 0f;
		GetComponent<CharacterController>().enabled = false;
		Camera.main.GetComponent<CharacterLook>().enabled = false;
		ScreenFade.Instance.StartFade(Color.red, 0.5f);
		yield return new WaitForSeconds(0.5f);
		GameManager.Instance.GameOver(killCount);
		ScreenFade.Instance.StartFade(Color.clear, 0.5f);
		
	}
}
