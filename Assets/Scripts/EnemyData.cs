using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData: ScriptableObject
{
    public float life;
    public float speed;
    public int damage;
}
