using UnityEngine;
using System.Collections;

public class BasicBullet : MonoBehaviour {
	
	public Projectile.Source source
	{
		get
		{
			return source;
		}
		set 
		{
			source = value;
		}
	}
	
	
	
	// Use this for initialization
	void Awake() 
	{
		source = Projectile.Source.Other;
		Projectile.AddProjectile(this.gameObject);
	}
	
	public void SetDamageSource(Projectile.Source s)
	{
		source = s;
	}
	
}
