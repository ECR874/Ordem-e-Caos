using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    public static event Action<EnemyData> OnEnemyReachedEnd;
    private Trial _currentTrial;
    
    private int _currentWaypoint;  
    private Vector3 _targetPosition;
    
    void Awake()
    {
        _currentTrial = GameObject.Find("Trial1").GetComponent<Trial>();
    }
    void OnEnable()
    {
        _currentWaypoint = 0;
        _targetPosition = _currentTrial.GetPosition(_currentWaypoint);
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
}   
