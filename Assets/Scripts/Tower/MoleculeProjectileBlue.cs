using UnityEngine;
using System.Collections;


public class MoleculeBlueProjectile : MonoBehaviour
{
    private Enemy target;
    private float dmg, slowPercent, slowDuration, speed;
    private int slowId;


    public void Init(Enemy t, float dmg, float slowPercent, float slowDuration, float speed)
    {
        this.target = t;
        this.dmg = dmg;
        this.slowPercent = slowPercent;
        this.slowDuration = slowDuration;
        this.speed = speed;
        slowId = GetInstanceID();
    }


    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }


        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);


        if (Vector3.Distance(transform.position, target.transform.position) < 0.05f)
        {
            target.TakeDamage(dmg);
            target.ApplySlow(slowId, slowPercent);
            StartCoroutine(RemoveSlowAfter());
            Destroy(gameObject);
        }
    }


    IEnumerator RemoveSlowAfter()
    {
        yield return new WaitForSeconds(slowDuration);
        if (target) target.RemoveSlow(slowId);
    }
}