using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
	public bool debug = false;
	public AudioClip firingSound;
	public float chargeTime = 10f; 		// time to charge from 0 to full
	public Transform particleEffect;
	
	private float charge = 10f;
	private Transform player;
	private bool firing = false;
	private float lastFireTime = 0f;
	
	// Use this for initialization
	void Start () 
	{
		particleEffect.parent = Camera.main.transform;
		particleEffect.localPosition = Vector3.zero;
		particleEffect.localRotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public void Fire()
	{
		lastFireTime = Time.time;
		if (!firing) StartCoroutine(FireLaser());
	}
	
	IEnumerator FireLaser()
	{
		firing = true;
		while (lastFireTime + 1f > Time.time)
		{
			
			yield return new WaitForFixedUpdate();
		}
		firing = false;
	}
	
	
}
