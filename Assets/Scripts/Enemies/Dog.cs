using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Dog : MonoBehaviour 
{
	public float moveSpeed;
	public int hitPoints = 10;
	
	private bool isDead = false;
	private Transform player;
	private Animator anim;

	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (isDead) return;
		Vector3 target = player.position;
		target.y = transform.position.y;
		Vector3 direction = target - transform.position;
		transform.rotation = Quaternion.LookRotation(direction);
		rigidbody.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (isDead) return;
		if (col.gameObject.tag == "Bullet")
		{
			if ( col.relativeVelocity.magnitude > 10f )
			{
				StartCoroutine(Hurt());
				if (hitPoints < 0) StartCoroutine(Die());
			}
		}
		if (col.gameObject.tag == "Player")
		{
			anim.Play("dog_attack");
		}
	}
	
	void OnCollisionExit(Collision col)
	{
		if (isDead) return;
		if (col.gameObject.tag == "Player")
		{
			anim.Play ("dog_walkcycle");
		}
			
	}
	
	IEnumerator Hurt()
	{
		if (!isDead)
		{
			hitPoints--;
			anim.Play("dog_hit");
			yield return new WaitForSeconds(0.4f);
			anim.Play ("dog_walkcycle");
		}
		yield return new WaitForFixedUpdate();
	}
		
	
	IEnumerator Die()
	{
		isDead = true;
		anim.Play("dog_death");
		yield return new WaitForSeconds(1f);
		Destroy(this.gameObject);
	}
}
