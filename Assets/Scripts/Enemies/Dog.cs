using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Dog : MonoBehaviour 
{
	public float moveSpeed;
	public int hitPoints = 10;
	
	private Transform player;

	// Use this for initialization
	void Awake () 
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Vector3 target = player.position;
		target.y = transform.position.y;
		Vector3 direction = target - transform.position;
		transform.rotation = Quaternion.LookRotation(direction);
		rigidbody.AddForce(transform.forward * moveSpeed * Time.deltaTime, ForceMode.VelocityChange);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Bullet")
		{
			if ( col.relativeVelocity.magnitude > 10f )
			{
				hitPoints--;
				if (hitPoints < 0) StartCoroutine(Die());
			}
		}
	}
	
	IEnumerator Die()
	{
		yield return new WaitForFixedUpdate();
		Destroy(this.gameObject);
	}
}
