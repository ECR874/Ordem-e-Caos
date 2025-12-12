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
    [SerializeField] private GameObject criation;
    [SerializeField] private GameObject aura1;
    [SerializeField] private GameObject aura2;

    public void NewGame()
    {
        title.enabled = false;
        title2.enabled = false;
        StartPanel.SetActive(true);
        newGameButton.SetActive(false);
        quitButton.SetActive(false);
        criation.SetActive(false);
        aura1.SetActive(false);
        aura2.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void NewGameText() 
    {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.levels[0]);
        Time.timeScale = 1f;
    }   
}