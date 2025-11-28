using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void NewGame()
    {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.levels[0]);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Apllication.Quit();
        #endif
    }
}
