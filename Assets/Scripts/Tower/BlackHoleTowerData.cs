using UnityEngine;

[CreateAssetMenu(fileName = "BlackHoleTowerData", menuName = "Scriptable Objects/BlackHoleTower")]
public class BlackHoleTowerData : BaseTowerData
{
    [System.Serializable]
    public class LevelData
    {
        public float range;
        public float slowPercentage;    
        public float damagePerSecond; 
        public Sprite sprite;
    }

    public LevelData[] levels;
}

