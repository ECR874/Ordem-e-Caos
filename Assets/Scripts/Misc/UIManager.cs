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
    [SerializeField] private TMP_Text ResourcesText;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject ResourcesPanel;
     [SerializeField] public AudioSource AS;
    [SerializeField] public AudioClip WaveBeep;
    private bool _isPaused = false;

    [SerializeField] private GameObject SpeedButtonsPanel;
    [SerializeField] private Button speed1Button;
    [SerializeField] private Button speed2Button;
    [SerializeField] private Button speed3Button;

    private void Start()
    {
        speed1Button.onClick.AddListener(() => SetGameSpeed(0.2f));
        speed2Button.onClick.AddListener(() => SetGameSpeed(1f));
        speed3Button.onClick.AddListener(() => SetGameSpeed(2f));
    }

    private void OnEnable()
    {
        Spawner.OnWaveChanged += UpdateWaveText;
        GameManager.OnLivesChanged += UpdateLivesText;
        GameManager.OnResourcesChanged += UpdateResourcesText;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
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

    private void UpdateResourcesText(int currentResources)
    {
        ResourcesText.text = $"energy of creation: {currentResources}";
    }

    public void PauseGame()
    {
        if(_isPaused){
            PauseButton.enabled = true;
            PauseButton.interactable = true;
            PB.SetActive(true);
            SpeedButtonsPanel.SetActive(true);
            PausePanel.SetActive(false);
            ResourcesPanel.SetActive(true);
            _isPaused = false;
            Time.timeScale = GameManager.Instance._gameSpeed;
        }
        else
        {
            Time.timeScale = 0f;
            PauseButton.enabled = false;
            PauseButton.interactable = false;
            PB.SetActive(false);
            SpeedButtonsPanel.SetActive(false);
            PausePanel.SetActive(true);
            ResourcesPanel.SetActive(false);
            _isPaused = true;
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0f;
        PB.SetActive(false);
        SpeedButtonsPanel.SetActive(false);
        ResourcesPanel.SetActive(false);
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


    private void SetGameSpeed(float time)
    {
        GameManager.Instance.SetGameSpeed(time);
    }
}
