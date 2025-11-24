using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static event Action<int> OnWaveChanged;

    [SerializeField] private WaveData[] waves;
    private int _currentWaveIndex = 0; 
    private int _waveCounter = 0;
    private WaveData CurrentWave => waves[_currentWaveIndex];
    
    private float _spawnTime;
    private float _spawnCounter;
    private int _enemiesRemoved;
    
    [SerializeField] private ObjectPooler antimatterPool;
    [SerializeField] private ObjectPooler conglomeratePool;
    [SerializeField] private ObjectPooler deathsTouchPool;
    [SerializeField] private ObjectPooler legionPool;
    [SerializeField] private ObjectPooler remnantPool;
    [SerializeField] private ObjectPooler voidHoundPool;
    
    private Dictionary<EnemyType, ObjectPooler> _poolDictionary;

    private float _wavePause = 2f;
    private float _waveCooldown;
    private float _extraEnemies = 0;
    private bool _betweenWaves = false;
    
    private void Awake()
    {
        _poolDictionary = new Dictionary<EnemyType, ObjectPooler>()
        {
            { EnemyType.Antimatter, antimatterPool },
            { EnemyType.Conglomerate, conglomeratePool },
            { EnemyType.DeathsTouch, deathsTouchPool },
            { EnemyType.Legion, legionPool },
            { EnemyType.Remnant, remnantPool },
            { EnemyType.VoidHound, voidHoundPool }
        };
    }

    private void OnEnable()
    {
        Enemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }
    
    private void OnDisable()
    {
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Start()
    {
        OnWaveChanged?.Invoke(_waveCounter);
    }
    
    void Update()
    {
        if (_betweenWaves)
        {
            _waveCooldown -= Time.deltaTime;
            if (_waveCooldown <= 0)
            {
                _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;
                _waveCounter++;
                OnWaveChanged?.Invoke(_waveCounter);
                _spawnCounter = 0;
                _enemiesRemoved = 0;
                _spawnTime = 0;
                _extraEnemies = 0;
                _betweenWaves = false;
            }
        }
        else
        {
            _spawnTime -= Time.deltaTime;
            if (_spawnTime <= 0 && _spawnCounter < CurrentWave.numberOfEnemies + _extraEnemies)
            {
                _spawnTime = CurrentWave.spawnInterval;
                SpawnEnemy();
                _spawnCounter++;
            }
            else if (_spawnCounter >= CurrentWave.numberOfEnemies && _enemiesRemoved >= CurrentWave.numberOfEnemies + _extraEnemies)
            {
                _betweenWaves = true;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (_poolDictionary.TryGetValue(CurrentWave.enemyType, out var pool))
        {
            GameObject spawnObject = pool.GetPooledObject();
            spawnObject.transform.position = transform.position;
            spawnObject.SetActive(true);
        }                                                               
    }
    
    public void SpawnInternalEnemy(EnemyType EnemySpawned, Vector3 position)
    {
        if (_poolDictionary.TryGetValue(EnemySpawned, out var pool))
        {
            GameObject spawnObject = pool.GetPooledObject();
            spawnObject.transform.position = position;
            spawnObject.SetActive(true);
            
        }
    }
    

    private void HandleEnemyReachedEnd(EnemyData data)
    {
        _enemiesRemoved++;
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        _enemiesRemoved++;
    }
}

