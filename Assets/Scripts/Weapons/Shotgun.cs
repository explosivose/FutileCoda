using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour 
{
	public bool debug = false;
	public AudioClip firingSound;		// sound to play when shooting
	public float rateOfFire = 2f;		// how many times to fire in one second
	public Transform projectile;		// the projectile to shoot!
	public float projectileSpeed;		// how fast the projectile will travel initially
	private Transform player;			// refer to the player for stuff
	private bool firing = false;		// whether or not we're currently firing the gun
	
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	// Use this for initialization
	void Start () 
	{
		
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
	
	public void Fire()
	{
		if (!firing) StartCoroutine(SingleShot());
	}
	
	IEnumerator SingleShot()
	{
		firing = true;
		
		AudioSource.PlayClipAtPoint(firingSound, player.position);
		
		// setup bullet spread
		float angle = 8f;
		float angleSpread = Mathf.Deg2Rad *  Mathf.Clamp(90f-angle,Mathf.Epsilon,90f-Mathf.Epsilon) ;
		float distance = Mathf.Tan(angleSpread);
		
		Vector3 nozzle = player.position + player.forward;
		nozzle += Random.onUnitSphere * 0.1f;
		
		for (int i = 0; i < 8; i++)
		{
			Vector2 pointInCircle = Random.insideUnitCircle;
			Vector3 bulletDirection = new Vector3(pointInCircle.x, pointInCircle.y, distance).normalized;
			bulletDirection = player.rotation * bulletDirection;
			Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection);
			Transform b = Instantiate(projectile, nozzle, bulletRotation) as Transform;
			b.rigidbody.AddForce(bulletDirection * projectileSpeed, ForceMode.VelocityChange);
			b.BroadcastMessage("SetDamageSource", Projectile.Source.Player);
		}
		
		yield return new WaitForSeconds(1/rateOfFire);
		
		firing = false;
	}
}
