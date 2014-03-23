using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public int maxHealth = 10;
	
	public bool Dead
	{
		get { return isDead; }
	}
	
	private Transform[] weaponInventory = new Transform[2];
	private int selected = 0;
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
	
	void Hurt()
	{
		if (isDead) return;
		health--;
		StartCoroutine(HurtEffect());
		if ( health < 0 ) StartCoroutine(Die() );
	}
	
	void Hurt(int dmg)
	{
		if (isDead) return;
		health-=dmg;
		StartCoroutine(HurtEffect());
		if (health < 0 ) StartCoroutine( Die () );
	}
	
	IEnumerator HurtEffect()
	{
		if (!isDead)
		{
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
		if (isDead) return;
		if (Input.GetButton("Fire1") && !GameManager.Instance.GameIsPaused)
		{
			weaponInventory[selected].BroadcastMessage("Fire");
		}
		if (Input.GetKeyUp(KeyCode.Q))
		{
			if (selected == 0) selected = 1;
			else selected = 0;
		}
		healthText.text = (health * 10).ToString();
		scoreText.text = killCount.ToString();
	}
	
	IEnumerator Die()
	{
		isDead = true;
		AudioListener.volume = 0f;
		GetComponent<CharacterController>().enabled = false;
		Camera.main.GetComponent<CharacterLook>().enabled = false;
		ScreenFade.Instance.StartFade(Color.red, 0.5f);
		yield return new WaitForSeconds(0.5f);
		GameManager.Instance.GameOver(killCount);
		ScreenFade.Instance.StartFade(Color.clear, 0.5f);
		
	}
}
