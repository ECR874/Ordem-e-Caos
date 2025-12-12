using System.Collections.Generic;
using UnityEngine;


public class Tower : MonoBehaviour
{
    [SerializeField] protected BaseTowerData data;
    protected CircleCollider2D _circleCollider;
    protected List<Enemy> _enemiesInRange;
    protected ObjectPooler _projectilePooler;
    private float _shootTimer;


    protected virtual void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        TowerData td = data as TowerData;
        _circleCollider.radius = td.range;


        _enemiesInRange = new List<Enemy>();
        _projectilePooler = GetComponent<ObjectPooler>();
        _shootTimer = td.shootInterval;
    }


    protected virtual void Update()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0)
        {
            _shootTimer = (data as TowerData).shootInterval;
            Shoot();
        }
    }


    protected virtual void Shoot() {}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            _enemiesInRange.Add(enemy);
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (_enemiesInRange.Contains(enemy))
            {
                _enemiesInRange.Remove(enemy);
            }
        }
    }
}