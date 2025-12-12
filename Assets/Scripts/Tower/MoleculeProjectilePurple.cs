using UnityEngine;


public class MoleculePurpleProjectile : MonoBehaviour
{
    private Enemy target;
    private float dmg, speed;


    public void Init(Enemy t, float dmg, float speed)
    {
        target = t;
        this.dmg = dmg;
        this.speed = speed;
    }


    void Update()
    {
        if (!target) { Destroy(gameObject); return; }
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.transform.position) < 0.05f)
        {
            target.TakeDamage(dmg);
            Destroy(gameObject);
        }
    }
}