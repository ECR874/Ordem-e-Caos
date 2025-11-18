using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private int _lives = 14;
    private int _resources = 0;
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnResourcesChanged;
    
    [SerializeField] public AudioSource AS;
    [SerializeField] public AudioClip Player_Hurt_SFX;

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
}
