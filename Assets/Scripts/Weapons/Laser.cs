using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour 
{
	public bool debug = false;
	public AudioClip startSound;
	public float maxCharge = 5f;
	public float chargeTime = 10f; 		// time to charge from 0 to full
	public float dischargeRate = 5f; 
	public float damageRate = 2f;
	
	
	private float charge = 5f;
	private Transform player;
	private bool firing = false;
	private bool damage = false;
	private float lastFireTime = 0f;
	private LineRenderer lr;
	private Vector3 laserStart;
	private Vector3 laserEnd;
	
	// Use this for initialization
	void Start () 
	{
		lr = GetComponent<LineRenderer>();
		lr.SetWidth(maxCharge, maxCharge);
		charge = maxCharge;
	}
	
	// Update is called once per frame
	void Update () 
	{
		laserStart = Camera.main.transform.position + Vector3.down;
		laserEnd =Camera.main.transform.forward * 100;
		lr.SetPosition(0, laserStart);
		lr.SetPosition(1, laserEnd);
		 
		if (debug && Input.GetButton("Fire1")) Fire ();
		if (firing)
		{
			audio.pitch = 0.5f + (charge / maxCharge);
		}
		else
		{
			lr.SetWidth(0f, 0f);
			if (charge < maxCharge)
				charge += maxCharge * Time.deltaTime * 1/chargeTime;
		}
	}
	
	public void Fire()
	{
		if (lastFireTime + 0.1f < Time.time) 
			AudioSource.PlayClipAtPoint(startSound, laserStart);
		
		lastFireTime = Time.time;
		
		if (!firing && charge > 0f) StartCoroutine(FireLaser());
	}
	
	IEnumerator FireLaser()
	{
		firing = true;
		audio.Play();
		while (lastFireTime + 0.1f > Time.time && charge > 0f)
		{
			lr.SetWidth(charge, charge * 2f);
			charge -= 0.1f;
			if (!damage) StartCoroutine(Damage ());
			yield return new WaitForSeconds(1/dischargeRate);
		}
		firing = false;
		audio.Stop ();
	}
	
	IEnumerator Damage()
	{
		damage = true;
		RaycastHit[] hits = Physics.CapsuleCastAll(laserStart, laserEnd, charge/2f, laserEnd, 100f);
		foreach (RaycastHit hit in hits)
		{
			if (hit.transform.tag == "Enemy")
			{
				hit.transform.BroadcastMessage("LaserDamage");
			}
		}
		yield return new WaitForSeconds(1/damageRate);
		damage = false;
	}
	
	
	
}
