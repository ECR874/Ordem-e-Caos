using UnityEngine;

[CreateAssetMenu(fileName = "BaseTowerData", menuName = "Scriptable Objects/BaseTowerData")]
public abstract class BaseTowerData : ScriptableObject
{
    public Sprite sprite;
    public int cost;
    public GameObject prefab;
}
