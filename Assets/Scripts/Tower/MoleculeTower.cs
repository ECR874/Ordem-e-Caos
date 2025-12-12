using UnityEngine;
using System.Collections.Generic;
public class MoleculeTower : Tower
{
    private MoleculeTowerData D;
    private int index = 0;
    private float fireCooldown = 0f;
    
    [SerializeField] private ObjectPooler bluePool;
    [SerializeField] private ObjectPooler orangePool;
    [SerializeField] private ObjectPooler purplePool;


    protected override void Start()
    {
        base.Start();
        D = data as MoleculeTowerData;
        _circleCollider.radius = D.range;
    }
    
    protected override void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f && _enemiesInRange.Count > 0)
        {
            Shoot();
            fireCooldown = 1f / Mathf.Max(0.01f, D.fireRate);
        }
    }


    protected override void Shoot()
    {
        Enemy target = _enemiesInRange[0];
        if (!target) return;


        switch (index)
        {
            case 0: ShootBlue(target); break;
            case 1: ShootOrange(target); break;
            case 2: ShootPurple(target); break;
        }
        index = (index + 1) % 3;
    }


    void ShootBlue(Enemy e)
    {
        var go = Instantiate(D.bluePrefab, transform.position, Quaternion.identity);
        var p = go.AddComponent<MoleculeBlueProjectile>();
        p.Init(e, D.blueDamage, D.blueSlowPercent, D.blueSlowDuration, D.projectileSpeed);
    }


    void ShootOrange(Enemy e)
    {
        var go = Instantiate(D.orangePrefab, transform.position, Quaternion.identity);
        var p = go.AddComponent<MoleculeOrangeProjectile>();
        p.Init(e, D.orangeDamage, D.waypointsToGoBack, D.projectileSpeed);
    }


    void ShootPurple(Enemy e)
    {
        var go = Instantiate(D.purplePrefab, transform.position, Quaternion.identity);
        var p = go.AddComponent<MoleculePurpleProjectile>();
        p.Init(e, D.purpleDamage, D.projectileSpeed);
    }
}