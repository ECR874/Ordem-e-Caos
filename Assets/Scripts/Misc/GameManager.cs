using System;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private int _lives = 14;
    public static event Action<int> OnLivesChanged;
    [SerializeField] public AudioSource AS;
    [SerializeField] public AudioClip Player_Hurt_SFX;

    private void OnEnable()
    {
        Enemy.OnEnemyReachedEnd += HandleEnemyReachedEnd;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
    }

    private void Start()
    {
        OnLivesChanged?.Invoke(_lives);
    }

    private void HandleEnemyReachedEnd(EnemyData data)
    {
        _lives = Mathf.Max(-1, _lives - data.damage);
        OnLivesChanged?.Invoke(_lives);
        AS.PlayOneShot(Player_Hurt_SFX);
        AS.Play();
    }
}
