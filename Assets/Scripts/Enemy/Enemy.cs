using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public EnemyData Data => data;
    public static event Action<EnemyData> OnEnemyReachedEnd;
    public static event Action<Enemy> OnEnemyDestroyed;
    
    private Trial _currentTrial;
    
    private int _currentWaypoint;  
    private Vector3 _targetPosition;
    private float _lives;

    [SerializeField] private Transform healthBar;
    private Vector3 _healthBarPosition;
    
    void Awake()
    {
        _currentTrial = GameObject.Find("Trial1").GetComponent<Trial>();
        _healthBarPosition = healthBar.localScale;

}
    void OnEnable()
    {
        _currentWaypoint = 0;
        _targetPosition = _currentTrial.GetPosition(_currentWaypoint);
        _lives = data.life;
        UpdateHealthBar();
    }
    
    void Update() 
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, data.speed * Time.deltaTime);
        
        float distance = Vector3.Distance(transform.position, _targetPosition);
        if (distance < 0.1f)
        {
            if (_currentWaypoint < _currentTrial.Waypoints.Length - 1)
            {
                _currentWaypoint++;
                _targetPosition = _currentTrial.GetPosition(_currentWaypoint);
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
        
        if (_lives <= 0)
        {
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
}   
