using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public Orc prefabOrc;
	public Troll prefabTroll;
    public Transform[] spawnPoints;
    public float spawnTime;

	private int i, gangCount = 10, trollSpawnPercent = 20;
	private bool modeTroll = false, modeWaves = false, modeGangs = false;
    private float timer, waveTimer, gangTimer;

    void Start()
    {
		waveTimer = GetRandomTimer();
		gangTimer = GetRandomTimer();
        timer = spawnTime;
		modeTroll = PlayerPrefs.GetInt("Trolls", 0) == 1;
		modeWaves = PlayerPrefs.GetInt("Waves", 0) == 1;
		modeGangs = PlayerPrefs.GetInt("Gangs", 0) == 1;
    }

    void Update()
    {
        timer -= Time.deltaTime;
		if (modeWaves)
			waveTimer -= Time.deltaTime;
		
		if (modeGangs)
			gangTimer -= Time.deltaTime;
		
		
        if (timer <= 0)
        {
			EnemySpawn(spawnPoints[ Random.Range(0, spawnPoints.Length-1) ].position);

            timer = spawnTime;
        }
		
		if (gangTimer <= 0 && modeGangs)	// спавн банды в какой-то точке
        {
			int spawnIndex = Random.Range(0, spawnPoints.Length-1);
			
			for (i = 1; i <= gangCount; i++)
				EnemySpawn(spawnPoints[ spawnIndex ].position);

            gangTimer = GetRandomTimer();
        }
		
		if (waveTimer <= 0 && modeWaves)	// волна - по всем спавнам генерится юнит
        {
			foreach(Transform point in spawnPoints)
				EnemySpawn(point.position);
			
            waveTimer = GetRandomTimer();
        }
    }
	
	float GetRandomTimer() 
	{
		return Random.Range(10f, 20f);
	}
	
	void EnemySpawn(Vector3 position)
	{
		if (modeTroll && Random.Range(0, 100) <= trollSpawnPercent)
			Instantiate(prefabTroll, position, Quaternion.identity);
		else
			Instantiate(prefabOrc, position, Quaternion.identity);
	}
}
