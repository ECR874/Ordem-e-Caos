using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text title2;
    [SerializeField] private GameObject StartPanel;
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject quitButton;
    
    public void NewGame()
    {
        title.enabled = false;
        title2.enabled = false;
        StartPanel.SetActive(true);
        newGameButton.SetActive(false);
        quitButton.SetActive(false);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Apllication.Quit();
        #endif
    }
    
    public void NewGameText() {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.levels[0]);
        Time.timeScale = 1f;
    }   
}
