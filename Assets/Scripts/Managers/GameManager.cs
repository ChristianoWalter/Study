using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform[] enemiesSpawnPoints;
    [SerializeField] GameObject enemy;
    [SerializeField] float timeToSpawn;
    [SerializeField] bool activeSpawn;
    private float currentTime;
    public int keysNeeded;
    int keyPicked;
    public Action<int> UpdateKeysQuantity;

    private void Start()
    {
        currentTime = timeToSpawn;
    }

    private void Update()
    {
        if (!activeSpawn) return;
        {
            
        }
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }
        else
        {
            currentTime = timeToSpawn;
            SpawnEnemies();
        }
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemiesSpawnPoints.Length; ++i) 
        { 
            Instantiate(enemy, enemiesSpawnPoints[i]);
        }
    }

    public void CollectedKey()
    {
        keyPicked++;
        UpdateKeysQuantity.Invoke(keyPicked);
    }


}
