using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Dog : MonoBehaviour 
{
	public float moveSpeed;
	public int hitPoints = 10;
	public float attackRate = 1f;
	public AudioClip deathSound;
	public AudioClip[] biteSound;
	public AudioClip[] painSound;
	public AudioClip[] growlSound;
	
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
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		
		Vector3 target = player.position;
		target.y = transform.position.y;
		Vector3 direction = target - transform.position;
		transform.rotation = Quaternion.LookRotation(direction);
		if (isDead || isHurt) return;
		rigidbody.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (isDead) return;
		if (col.gameObject.tag == "Bullet")
		{
			if ( col.relativeVelocity.magnitude > 10f )
			{
				hitPoints-= 9;
				if (hitPoints < 0)
					lastShotBy = col.gameObject.GetComponent<BasicBullet>().ProjectileSource;
				if (!isHurt) StartCoroutine(HurtEffect());
			}
		}
		if (col.gameObject.tag == "Player")
		{
			if (!isAttacking) StartCoroutine(Attack(col.transform));
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if (isDead) return;
		if (col.gameObject.tag == "Player")
		{
			anim.Play ("dog_walkcycle");
			isAttacking = false;
		}
			
	}
	
	public void LaserDamage()
	{
		hitPoints--;
		lastShotBy = Projectile.Source.Player;
		if (!isHurt) StartCoroutine(HurtEffect());
	}
	
	IEnumerator Attack(Transform player)
	{
		isAttacking = true;
		anim.Play("dog_attack");
		while(isAttacking)
		{
			player.BroadcastMessage("Hurt");
			int i = Random.Range(0, biteSound.Length);
			AudioSource.PlayClipAtPoint(biteSound[i], transform.position);
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
				anim.Play("dog_hit");
				int j = Random.Range(0, painSound.Length);
				AudioSource.PlayClipAtPoint(painSound[j], transform.position);
				yield return new WaitForSeconds(0.334f);
				anim.Play ("dog_walkcycle");
			}
		}
		yield return new WaitForFixedUpdate();
		isHurt = false;
	}
		
	
	IEnumerator Die()
	{
		isDead = true;
		AudioSource.PlayClipAtPoint(deathSound, transform.position);
		anim.Play("dog_death");
		if (lastShotBy == Projectile.Source.Player)
			player.GetComponent<Player>().AddKill();
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
		
	}
}
