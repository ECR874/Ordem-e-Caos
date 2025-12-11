using UnityEngine;
using System.Collections.Generic;

public class BlackHoleTower : MonoBehaviour
{
    [SerializeField] private BlackHoleTowerData data;

    private CircleCollider2D _circleCollider;
    private List<Enemy> _enemiesInRange = new List<Enemy>();

    private int _currentLevel = 0;
    
    private int _enemiesSlowedCount = 0;
    private HashSet<Enemy> _alreadyCounted = new HashSet<Enemy>();
    
    private int SourceId => GetInstanceID();

    private void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        if (_circleCollider == null)
        {
            enabled = false;
            return;
        }
        ApplyLevelStats();
    }

    private void Update()
    {
        ApplyEffects();
    }

    private void ApplyLevelStats()
    {
        var level = data.levels[_currentLevel];

        _circleCollider.radius = level.range;

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = level.sprite;
    }

    public void Upgrade()
    {
        if (_currentLevel < data.levels.Length - 1)
        {
            _currentLevel++;
            ApplyLevelStats();
        }
    }

    private void ApplyEffects()
    {
        var level = data.levels[_currentLevel];
        
        float slowAmount = level.slowPercentage;

        for (int i = _enemiesInRange.Count - 1; i >= 0; i--)
        {
            var enemy = _enemiesInRange[i];
            if (enemy == null)
            {
                _enemiesInRange.RemoveAt(i);
                continue;
            }
            
            if (!_alreadyCounted.Contains(enemy))
            {
                _alreadyCounted.Add(enemy);
                _enemiesSlowedCount++;
                CheckLevelUp(); // ← novo método
            }
            
            enemy.ApplySlow(SourceId, slowAmount);
            
            if (level.damagePerSecond > 0f)
            {
                enemy.TakeDamage(level.damagePerSecond * Time.deltaTime);
            }
        }
    }
    
    private void CheckLevelUp()
    {
        if (_currentLevel == 0 && _enemiesSlowedCount >= 10)
        {
            Upgrade();
            return;
        }
        
        if (_currentLevel == 1 && _enemiesSlowedCount >= 10)
        {
            Upgrade();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        Enemy e = collision.GetComponent<Enemy>();
        if (e != null && !_enemiesInRange.Contains(e))
            _enemiesInRange.Add(e);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        Enemy e = collision.GetComponent<Enemy>();
        if (e != null && _enemiesInRange.Contains(e))
        {
            e.RemoveSlow(SourceId);
            _enemiesInRange.Remove(e);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _enemiesInRange.Count; i++)
        {
            var e = _enemiesInRange[i];
            if (e != null) e.RemoveSlow(SourceId);
        }
        _enemiesInRange.Clear();
    }
}
