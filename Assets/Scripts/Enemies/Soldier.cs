using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Soldier : MonoBehaviour 
{
	public float moveSpeed;
	public int hitPoints = 10;
	public float attackRate = 1f;
	public AudioClip deathSound;
	
	private bool isDead = false;
	private bool isHurt = false;
	private bool isAttacking = false;
	private Transform player;
	private Animator anim;
	private Projectile.Source lastShotBy;
	
	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		anim = GetComponent<Animator>();
		
	}
	
	void Start()
	{
		transform.BroadcastMessage("SetGunman", transform);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		Vector3 target = player.position;
		target.y = transform.position.y;
		Vector3 direction = target - transform.position;
		transform.rotation = Quaternion.LookRotation(direction);
		if (isDead || isHurt) return;
		
		// can I see the player?
		RaycastHit hit;
		if (Physics.Raycast(transform.position, direction, out hit, 10f))
		{
			if (hit.transform.tag == "Player")
			{
				if (!isAttacking) StartCoroutine(Attack());
			}
			else
			{
				isAttacking = false;
			}
		}
		if (isAttacking) return;
		rigidbody.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (isDead) return;
		if (col.gameObject.tag == "Bullet")
		{
			if ( col.relativeVelocity.magnitude > 10f )
			{
				hitPoints-= 4;
				if (hitPoints < 0)
					lastShotBy = col.gameObject.GetComponent<BasicBullet>().ProjectileSource;
				if (!isHurt) StartCoroutine(HurtEffect());
			}
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if (isDead) return;
		
	}
	
	public void LaserDamage()
	{
		hitPoints--;
		lastShotBy = Projectile.Source.Player;
		if (!isHurt) StartCoroutine(HurtEffect());
	}
	
	IEnumerator Attack()
	{
		isAttacking = true;
		anim.StopPlayback ();
		while(isAttacking)
		{
			transform.BroadcastMessage("Fire");
			yield return new WaitForSeconds(1/attackRate);
		}
		
	}
	
	IEnumerator HurtEffect()
	{
		isHurt = true;
		if (!isDead)
		{
			if (hitPoints < 0) 
			{
				StartCoroutine(Die ());
			}
			else
			{
				
				yield return new WaitForSeconds(0.334f);
				
			}
		}
		yield return new WaitForFixedUpdate();
		isHurt = false;
	}
	
	
	IEnumerator Die()
	{
		isDead = true;
		isAttacking = false;
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		anim.Play("soldier_death");
		if (lastShotBy == Projectile.Source.Player)
			player.GetComponent<Player>().AddKill();
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
		
	}
}
