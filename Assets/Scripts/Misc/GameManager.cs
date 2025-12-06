using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int _lives;
    private int _resources;
    public int Resources => _resources;
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnResourcesChanged;
    
    [SerializeField] public AudioSource AS;
    [SerializeField] public AudioClip Player_Hurt_SFX;

    public float _gameSpeed = 1f;
    public float GameSpeed => _gameSpeed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
        Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        OnLivesChanged?.Invoke(_lives);
        OnResourcesChanged?.Invoke(_resources);
    }

    private void HandleEnemyReachedEnd(EnemyData data)
    {
        _lives = Mathf.Max(-1, _lives - data.damage);
        OnLivesChanged?.Invoke(_lives);
        AS.PlayOneShot(Player_Hurt_SFX);
        AS.Play();
    }

    private void HandleEnemyDestroyed(Enemy enemy)
    {
        AddResources(Mathf.RoundToInt(enemy.Data.resourceReward));
    }
    
    private void AddResources(int amount)
    {
        _resources += amount;
        OnResourcesChanged?.Invoke(_resources);
    }

    public void SetGameSpeed(float newSpeed)
    {
        _gameSpeed = newSpeed;
        Time.timeScale = _gameSpeed;
    }

    public void ResetGameState()
    {
        _lives = LevelManager.Instance.CurrentLevel.startingLives;
        OnLivesChanged?.Invoke(_lives);
        _resources = LevelManager.Instance.CurrentLevel.startingResources;
        OnResourcesChanged?.Invoke(_resources);
        
        SetGameSpeed(1f);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetGameState();
    }
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
<<<<<<< Updated upstream
    
    public void SpendResources(int amount)
=======

    public void SpendResources (int amount)
>>>>>>> Stashed changes
    {
        if (_resources >= amount)
        {
            _resources -= amount;
            OnResourcesChanged?.Invoke(_resources);
        }
<<<<<<< Updated upstream
    } 
=======
    }
>>>>>>> Stashed changes
}

