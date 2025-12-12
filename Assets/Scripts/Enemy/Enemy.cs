using UnityEngine;
using System;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public EnemyData Data => data;
    public static event Action<EnemyData> OnEnemyReachedEnd;
    public static event Action<Enemy> OnEnemyDestroyed;
    
    private Trial _currentTrial;

    private int _currentW;
    public int CurrentWaypoint 
    {
        get { return _currentW; }
        set {
            _currentW = value;
            _targetPosition = _currentTrial.GetPosition(_currentW);
        }
    }

    private Vector3 _targetPosition;
    private float _lives;
    
    private EnemySpawner spawner;
    private SpeedChanger speedChanger;
    private float _currentSpeed;
    private float _baseSpeed;
    
    private Dictionary<int, float> slows = new Dictionary<int, float>();
    
    private bool speedIncreased = false;
    
    [SerializeField] private Transform healthBar;
    private Vector3 _healthBarPosition;
    
    void Awake()
    {
        _currentTrial = GameObject.Find("Trial1").GetComponent<Trial>();
        _healthBarPosition = healthBar.localScale;
        spawner = GetComponent<EnemySpawner>();
        speedChanger = GetComponent<SpeedChanger>();

}
    void OnEnable()
    {
        CurrentWaypoint = 0;
        _lives = data.life;
        _baseSpeed = data.speed;
        _currentSpeed = data.speed;
        UpdateHealthBar();
        slows.Clear();
    }
    
    void Update() 
    {
        UpdateFinalSpeedFromSlows();
        
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _currentSpeed * Time.deltaTime);
        
        float distance = Vector3.Distance(transform.position, _targetPosition);
        if (distance < 0.1f)
        {
            if (CurrentWaypoint < _currentTrial.Waypoints.Length - 1)
            {
                CurrentWaypoint++;
            }
            else
            {
                OnEnemyReachedEnd?.Invoke(data);
                gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        _lives -= damage;
        _lives = Math.Max(_lives, 0);
        UpdateHealthBar();

        if (!speedIncreased && _lives <= data.life * 0.5f)
        {
            if (speedChanger != null)
            {
                _currentSpeed = speedChanger.ChangeSpeed(_currentSpeed);
                speedIncreased = true;
            }
        }
        
        if (_lives <= 0)
        {
            if (spawner != null)
            {
                spawner.SpawnOnDeath(transform.position);
            } 
            OnEnemyDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBar()
    {
        float healthPercent = _lives / data.life;
        Vector3 scale = _healthBarPosition;
        scale.x = _healthBarPosition.x * healthPercent;
        healthBar.localScale = scale;
    }
    
    public void ApplySlow(int sourceId, float slowPercent)
    {
        if (slows.ContainsKey(sourceId))
            slows[sourceId] = slowPercent;
        else
            slows.Add(sourceId, slowPercent);
    }

    public void RemoveSlow(int sourceId)
    {
        if (slows.ContainsKey(sourceId))
            slows.Remove(sourceId);
    }

    private void UpdateFinalSpeedFromSlows()
    {
        if (slows.Count == 0)
        {
            _currentSpeed = _baseSpeed;
            return;
        }
        
        float maxSlow = 0f;

        foreach (var s in slows.Values)
        {
            if (s > maxSlow)
                maxSlow = s;
        }
        
        _currentSpeed = _baseSpeed * (1f - maxSlow);
    }
} 