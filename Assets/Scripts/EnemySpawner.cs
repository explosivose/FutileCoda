using UnityEngine;
using System.Collections;

public class EnemySpawner : Singleton<EnemySpawner> 
{

	public Transform[] enemies;
	public float spawnTime; 
	private GameObject[] spawners;

	// Use this for initialization
	void Start () 
	{
		spawners = GameObject.FindGameObjectsWithTag("Spawner");
		
	}
	
	public void StartSpawning()
	{
		StartCoroutine(SpawnLoop());
	}
	
			
	IEnumerator SpawnLoop()
	{
		while(true)
		{
			SpawnSingleRandom();
			yield return new WaitForSeconds(spawnTime);
		}
	}
		
	
	void SpawnSingleRandom()
	{
		int point = Random.Range(0, spawners.Length);
		int enemy = Random.Range(0, enemies.Length);
		Instantiate(enemies[enemy], spawners[point].transform.position, Quaternion.identity);
	}
	
}
