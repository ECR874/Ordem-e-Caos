using System;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public static event Action<int> OnWaveChanged;
    public static event Action OnMissionComplete;

    [SerializeField] private WaveData[] waves;
    private int _currentWaveIndex = 0;
    private int _waveCounter = 0;
    private WaveData CurrentWave => waves[_currentWaveIndex];
    
    private int _enemiesRemoved;
    private float _wavePause = 2f;
    private float _waveCooldown;
    private bool _betweenWaves = false;
    
    [SerializeField] private ObjectPooler antimatterPool;
    [SerializeField] private ObjectPooler conglomeratePool;
    [SerializeField] private ObjectPooler deathsTouchPool;
    [SerializeField] private ObjectPooler legionPool;
    [SerializeField] private ObjectPooler remnantPool;
    [SerializeField] private ObjectPooler voidHoundPool;

    private Dictionary<EnemyType, ObjectPooler> _poolDictionary;

    private List<EnemyType> _enemies;
    private int _currentGroupIndex;
    private int _index;
    private float _spawnTimer;
    private float _groupDelay;
    public int _totalEnemiesInWave;
    

    private void Awake()
    {
        Instance = this;

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
        PrepareWave();
    }

    private void PrepareWave()
    {
        _currentWaveIndex = 0;
        _enemies = new List<EnemyType>();
        _index = 0;
        _enemiesRemoved = 0;
        _totalEnemiesInWave = 0;

        foreach(var group in CurrentWave.groups) 
            foreach(var sg in group.subGroups)
                _totalEnemiesInWave += sg.count;

        PrepareGroup();
    }

    private void PrepareGroup()
    {
        if (_currentGroupIndex >= CurrentWave.groups.Length)
        {
            return;
        }
        
        _enemies = new List<EnemyType>();
        _index = 0;
        
        var group = CurrentWave.groups[_currentGroupIndex];

        foreach(var sg in group.subGroups)
        {
            for(int i = 0; i < sg.count; i++)
            {
                _enemies.Add(sg.enemyType);
            }
        }

        for (int i = 0; i < _enemies.Count; i++)
        {
            int r = UnityEngine.Random.Range(0, _enemies.Count);
            (_enemies[i], _enemies[r]) = (_enemies[r], _enemies[i]);
        }

        _spawnTimer = 0;
        _groupDelay = group.groupDelay;
    }

    void Update()
    {
        if (_betweenWaves)
        {
            _waveCooldown -= Time.deltaTime;
            if (_waveCooldown <= 0)
            {
                if (_waveCounter + 1 >= LevelManager.Instance.CurrentLevel.wavesToWin)
                {
                    OnMissionComplete?.Invoke();
                    return;
                }
                _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;
                _waveCounter++;
                OnWaveChanged?.Invoke(_waveCounter);

                _betweenWaves = false;
            }
            return;
        }

        if (_currentGroupIndex >= CurrentWave.groups.Length)
        {
            if (_enemiesRemoved >= _totalEnemiesInWave)
            {
                _betweenWaves = true;
                _waveCooldown = _wavePause;
            }

            return;
        }

        if (_index >= _enemies.Count)
        {
            _groupDelay -= Time.deltaTime;
            if (_groupDelay <= 0)
            {
                _currentGroupIndex++;
                PrepareGroup();
            }

            return;
        }
        
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            SpawnEnemy(_enemies[_index]);
            _index++;
            _spawnTimer = CurrentWave.spawnInterval;
        }
    }
    
    private void SpawnEnemy(EnemyType enemyType)
    {
        if (_poolDictionary.TryGetValue(enemyType, out var pool))
        {
            GameObject obj = pool.GetPooledObject();
            obj.transform.position = transform.position;
            obj.SetActive(true);
        }
    }

    public Enemy SpawnInternalEnemy(EnemyType enemyType, Vector3 pos)
    {
        if (_poolDictionary.TryGetValue(enemyType, out var pool))
        {
            GameObject obj = pool.GetPooledObject();

            obj.transform.position = pos;
            obj.SetActive(true);

            return obj.GetComponent<Enemy>();
        }

        return null;
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
