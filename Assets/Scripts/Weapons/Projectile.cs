using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile 
{
	public enum Source
	{
		Player,
		Enemy,
		Other
	}
	
	private static int maximumProjectiles = 255;
	private static Queue<GameObject> projectiles = new Queue<GameObject>();
	
	public static void AddProjectile(GameObject p)
	{
		projectiles.Enqueue(p);
		if (projectiles.Count > maximumProjectiles)
		{
			GameObject next = projectiles.Peek();
			projectiles.Dequeue();
			GameObject.Destroy(next);
		}
	}
	

}
