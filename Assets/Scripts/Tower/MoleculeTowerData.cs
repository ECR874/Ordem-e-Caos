using UnityEngine;


[CreateAssetMenu(fileName = "MoleculeTower", menuName = "Scriptable Objects/Molecule Tower Data")]
public class MoleculeTowerData : BaseTowerData
{
    public float range;
    public float projectileSpeed;
    public float fireRate;


    public GameObject bluePrefab;
    public float blueDamage;
    public float blueSlowPercent;
    public float blueSlowDuration;


    public GameObject orangePrefab;
    public float orangeDamage;
    public int waypointsToGoBack;


    public GameObject purplePrefab;
    public float purpleDamage;
}