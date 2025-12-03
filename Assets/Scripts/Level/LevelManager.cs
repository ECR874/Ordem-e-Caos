using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    public LevelData[] levels;
    public LevelData CurrentLevel { get; private set; }

    private int _initialLives;
    private int _initialResources;
    private int _initialWaves;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        CurrentLevel = levels[0];
    }

    public void LoadLevel(LevelData levelData)
    {
        CurrentLevel = levelData;
        SceneManager.LoadScene(levelData.levelName);
    }
}