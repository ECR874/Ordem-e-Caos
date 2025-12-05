using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject PausePanel;
    [SerializeField] private GameObject PB;
    [SerializeField] private Button PauseButton;
    [SerializeField] private TMP_Text LivesText;
    [SerializeField] private TMP_Text WavesText;
    [SerializeField] private TMP_Text ResourcesText;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject missionCompletePanel;
    [SerializeField] private GameObject ResourcesPanel;
    [SerializeField] public AudioSource AS;
    [SerializeField] public AudioClip WaveBeep;
    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardConatiner;
    [SerializeField] private TowerData[] towers;
    private List<GameObject> activeCards = new List<GameObject>();
    private Platform _currentPlatform;
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
        Spawner.OnMissionComplete += ShowMissionComplete;
        Platform.OnPlatformClicked += HandlePlatformClick;
        TowerCard.OnTowerSelected += HandleTowerSelected;
    }

    private void OnDisable()
    {
        Spawner.OnWaveChanged -= UpdateWaveText;
        GameManager.OnLivesChanged -= UpdateLivesText;
        GameManager.OnResourcesChanged -= UpdateResourcesText;
        Spawner.OnMissionComplete -= ShowMissionComplete;
        Platform.OnPlatformClicked -= HandlePlatformClick;
    }

    private void UpdateLivesText(int currentLives)
    {
        LivesText.text = $"health: {currentLives}";

        if (currentLives < 0)
        {
            GameOver();
        }
    }

    private void UpdateWaveText(int currentWave)
    {
        WavesText.text = $"cicle: {currentWave + 1}/20";
        AS.PlayOneShot(WaveBeep);
        AS.Play();
    }

    private void UpdateResourcesText(int currentResources)
    {
        ResourcesText.text = $"energy of creation: {currentResources}";
    }

    private void HandlePlatformClick(Platform platform)
    {
        _currentPlatform = platform;
        ShowTowerPanel();
    }
    public void ShowTowerPanel()
    {
        towerPanel.SetActive(true);
        GameManager.Instance.SetTimeScale(0f);
        PopulateTowerCards();
    }

    public void HideTowerPanel()
    {
        towerPanel.SetActive(false);
        GameManager.Instance.SetTimeScale(1f);
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
        LevelManager.Instance.LoadLevel(LevelManager.Instance.CurrentLevel);
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

    private void ShowMissionComplete()
    {
        missionCompletePanel.SetActive(true);
        SpeedButtonsPanel.SetActive(false);
        ResourcesPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        PauseButton.enabled = false;
        PauseButton.interactable = false;
        LivesText.enabled = false;
        WavesText.enabled = false;
        Time.timeScale = 0f;
    }
    
    public void GoToLevel2()
    {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.levels[1]);

    }
    
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");

    }

    private void PopulateTowerCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        foreach (var data in towers)
        {
            GameObject cardGameObject = Instantiate(towerCardPrefab, cardConatiner);
            TowerCard card = cardGameObject.GetComponent<TowerCard>();
            card.Initialize(data);
            activeCards.Add(cardGameObject);
        }
        
    }

    private void HandleTowerSelected(TowerData towerData)
    {
        _currentPlatform.PlaceTower(towerData);
        HideTowerPanel();  
    }
}
