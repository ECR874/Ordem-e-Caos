using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject PB;
    [SerializeField] private Button PauseButton;
    [SerializeField] private TMP_Text LivesText;
    [SerializeField] private TMP_Text WavesText;
    [SerializeField] private GameObject GameOverPanel;
     [SerializeField] public AudioSource AS;
    [SerializeField] public AudioClip WaveBeep;
    private bool _isPaused = false;

    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
    }

    private void UpdateLivesText(int currentLives)
    {
        LivesText.text = $"health: {currentLives + 1}";

        if (currentLives < 0)
        {
            GameOver();
        }
    }

    private void UpdateWaveText(int currentWave)
    {
        WavesText.text = $"wave: {currentWave + 1}";
        AS.PlayOneShot(WaveBeep);
        AS.Play();
    }

    public void PauseGame()
    {
        if(_isPaused){
            PauseButton.enabled = true;
            PauseButton.interactable = true;
            PB.SetActive(true);
            PausePanel.SetActive(false);
            _isPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            PauseButton.enabled = false;
            PauseButton.interactable = false;
            PB.SetActive(false);
            PausePanel.SetActive(true);
            _isPaused = true;
            Time.timeScale = 0f;
        }
    }

    private void GameOver()
    {
        PB.SetActive(false);
        Time.timeScale = 0f;
        GameOverPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Apllication.Quit();
            #endif
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
