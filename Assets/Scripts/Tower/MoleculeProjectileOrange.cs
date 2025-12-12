using UnityEngine;


public class MoleculeOrangeProjectile : MonoBehaviour
{
    private Enemy target;
    private float dmg, speed;
    private int stepsBack;


    public void Init(Enemy t, float dmg, int stepsBack, float speed)
    {
        this.target = t;
        this.dmg = dmg;
        this.stepsBack = stepsBack;
        this.speed = speed;
    }


    void Update()
    {
        if (!target) { Destroy(gameObject); return; }
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.transform.position) < 0.05f)
        {
            target.TakeDamage(dmg);
            target.ForceGoBack(stepsBack);
            Destroy(gameObject);
        }
    }
}