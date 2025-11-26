using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Scriptable Objects/WaveData")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class SubGroup
    {
        public EnemyType enemyType;
        public int count;
    }

    [System.Serializable]
    public class Group
    {
        public SubGroup[] subGroups;
        public float groupDelay;
    }
    
    public Group[] groups;
    public float spawnInterval;
}
