using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class Constants
{
    public const string KeyHighScore = "HighScoreLevel1";
}

public enum GameState
{
    [InspectorName("Gameplay")] GAME,
    [InspectorName("Pause")] PAUSE_MENU,
    [InspectorName("Game Over")] GAME_OVER,
    [InspectorName("Level completed")] LEVEL_COMPLETED,
    [InspectorName("Options")] OPTIONS
}

public class GameManager : MonoBehaviour
{
    private int score;
    private int highScore;
    private int keysFound;
    private int lives = 3;
    private int qualityIndex;

    public static GameManager Instance;
    [SerializeField] public GameState currentGameState = GameState.GAME;
    [SerializeField] public Canvas inGameCanvas;
    [SerializeField] public Canvas gameOverCanvas;
    [SerializeField] public Canvas pauseMenuCanvas;
    [SerializeField] public Canvas levelCompletedCanvas;
    [SerializeField] public Canvas optionsCanvas;
    [SerializeField] public TMP_Text scoreText;
    [SerializeField] public TMP_Text keysFoundText;
    [SerializeField] public TMP_Text finalScoreText;
    [SerializeField] public TMP_Text bestScoreText;
    [SerializeField] public TMP_Text qualityText;
    [SerializeField] public Image[] keyTabs;
    [SerializeField] public Image[] hearts;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitGame();
    }

    private void InitGame()
    {
        SetDefaultGameState();
        SetDefaultCanvas();
        SetDefaultScore();
        SetDefaultKeysFound();
        SetDefaultHearts();
        pauseMenuCanvas.enabled = false;
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        qualityIndex = QualitySettings.GetQualityLevel();
        foreach (var keytab in keyTabs) keytab.color = Color.gray;
        if (!PlayerPrefs.HasKey(Constants.KeyHighScore)) PlayerPrefs.SetInt(Constants.KeyHighScore, 0);
    }

    private void Update()
    {
        OnEscapeKey();
        SetTimescale();
    }

    private void OnEscapeKey()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        switch (currentGameState)
        {
            case GameState.PAUSE_MENU:
                InGame();
                break;
            case GameState.GAME:
                PauseMenu();
                break;
            case GameState.LEVEL_COMPLETED:
                OnExitButtonClicked();
                break;
            case GameState.GAME_OVER:
                OnExitButtonClicked();
                break;
            default:
                InGame();
                break;
        }
    }

    private void SetDefaultCanvas()
    {
        SetCanvas(currentGameState);
    }

    private void SetDefaultGameState()
    {
        currentGameState = GameState.GAME;
    }

    private void SetDefaultScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    private void SetDefaultKeysFound()
    {
        keysFound = 0;
        keysFoundText.text = keysFound.ToString();
    }

    private void SetDefaultHearts()
    {
        lives = 3;
        foreach (var heart in hearts) heart.enabled = true;
    }

    private void SetTimescale()
    {
        Time.timeScale = currentGameState == GameState.GAME ? 1 : 0;
    }

    private void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (newGameState == GameState.LEVEL_COMPLETED)
        {
            var currentScene = SceneManager.GetActiveScene();
            if (currentScene.name == "Level 1")
            {
                highScore = PlayerPrefs.GetInt(Constants.KeyHighScore);

                if (highScore < score)
                {
                    PlayerPrefs.SetInt(Constants.KeyHighScore, score);
                    highScore = score;
                    finalScoreText.text = score.ToString();
                    bestScoreText.text = highScore.ToString();
                }
                else
                {
                    finalScoreText.text = score.ToString();
                    bestScoreText.text = highScore.ToString();
                }
            }
        }

        SetCanvas(newGameState);
    }

    private void SetCanvas(GameState newGameState)
    {
        inGameCanvas.enabled = newGameState == GameState.GAME;
        pauseMenuCanvas.enabled = newGameState == GameState.PAUSE_MENU;
        gameOverCanvas.enabled = newGameState == GameState.GAME_OVER;
        levelCompletedCanvas.enabled = newGameState == GameState.LEVEL_COMPLETED;
        optionsCanvas.enabled = newGameState == GameState.OPTIONS;
    }

    private void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
    }

    private void InGame()
    {
        SetGameState(GameState.GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }

    private void GameOver()
    {
        SetGameState(GameState.GAME_OVER);
    }

    private void Options()
    {
        SetGameState(GameState.OPTIONS);
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void AddKeys(int keys)
    {
        keyTabs[keysFound].color = Color.white;
        keysFound += keys;
        keysFoundText.text = keysFound.ToString();
    }

    public void AddHeart()
    {
        lives++;
        hearts[lives - 1].enabled = true;
    }

    public void RemoveHeart()
    {
        lives--;
        hearts[lives].enabled = false;
        if (lives == 0) GameOver();
    }

    public void EndGateHit()
    {
        if (keysFound >= 3) LevelCompleted();
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        InitGame();
    }

    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnOptionsButtonClicked()
    {
        Options();
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetQuality(bool increase)
    {
        if (increase)
        {
            qualityIndex++;
            if (qualityIndex > QualitySettings.names.Length-1)
            {
                qualityIndex = QualitySettings.names.Length - 1;
            }
            QualitySettings.SetQualityLevel(qualityIndex);
            qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
           
        }
        else
        {
            qualityIndex--;
            if (qualityIndex < 0)
            {
                qualityIndex = 0;
            }
            QualitySettings.SetQualityLevel(qualityIndex);
            qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
        }
    }
}