
using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Enemy myCharacter;
    [SerializeField] private EnemyType enemyToSpawn;
    [SerializeField] private int enemiesToSpawnOnDeath;
    

    private void Awake()
    {
        myCharacter = GetComponent<Enemy>();
    }
    public void SpawnOnDeath(Vector3 deathPosition)
    {
        for (int i = 0; i < enemiesToSpawnOnDeath; i++)
        {
            var e = Spawner.Instance.SpawnInternalEnemy(enemyToSpawn, deathPosition);
            e.CurrentWaypoint = myCharacter.CurrentWaypoint;
            Spawner.Instance._totalEnemiesInWave++;
        }
    }
}