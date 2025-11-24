
using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyType enemyToSpawn;
    [SerializeField] private int enemiesToSpawnOnDeath;
    
    private Spawner internalSpawn;

    private void Awake()
    {
        internalSpawn = FindObjectOfType<Spawner>();
    }
    public void SpawnOnDeath(Vector3 deathPosition)
    {
        for (int i = 0; i < enemiesToSpawnOnDeath; i++)
        {
            internalSpawn.SpawnInternalEnemy(enemyToSpawn, deathPosition);
        }
    }

}