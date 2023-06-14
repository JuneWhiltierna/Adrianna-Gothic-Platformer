using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public enum GameState
{
    [InspectorName("Gameplay")] GAME,
    [InspectorName("Pause")] PAUSE_MENU,
    [InspectorName("Level completed")] LEVEL_COMPLETED
}

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private int keysFound = 0;
    private int lives = 3;
    private int enemiesDefeated = 0;


    public static GameManager Instance;

    public GameState currentGameState = GameState.GAME;

    public Canvas InGameCanvas;
    public Canvas GameOverCanvas;
    public Canvas pauseMenuCanvas;
    public TMP_Text scoreText;
    public TMP_Text keysFoundText;
    public TMP_Text enemiesText;
    public TMP_Text levelCompletedText;
    public TMP_Text gameOverText;
    public TMP_Text gamePausedText;
    [FormerlySerializedAs("keyTab")] public Image[] keyTabs;
    public Image[] hearts;

    private void Awake()
    {
        ProvideInstance();
        SetDefaultCanvas();
        SetDefaultTexts();
        SetDefaultScore();
        SetDefaultKeysFound();
        SetDefaultEnemies();
        SetDefaultHearts();
        pauseMenuCanvas.enabled = false;

        foreach (var keytab in keyTabs)
        {
            keytab.color = Color.gray;
        }
    }

    private void Update()
    {
        OnEscapeKey();
    }

    private void OnEscapeKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentGameState)
            {
                case GameState.PAUSE_MENU:
                    InGame();
                    break;
                case GameState.GAME:
                    PauseMenu();
                    break;
                case GameState.LEVEL_COMPLETED:
                    break;
                default:
                    InGame();
                    break;
            }
        }
    }

    private void SetDefaultCanvas()
    {
        SetCanvas(currentGameState);
    }

    private void SetDefaultScore()
    {
        scoreText.text = score.ToString();
    }

    private void SetDefaultKeysFound()
    {
        keysFoundText.text = keysFound.ToString();
    }

    private void SetDefaultHearts()
    {
        foreach (var heart in hearts)
        {
            heart.enabled = true;
        }
    }

    private void SetDefaultEnemies()
    {
        // enemiesText.text = enemiesDefeated.ToString();
    }

    private void SetDefaultTexts()
    {
        //levelCompletedText.enabled = false;
        // gameOverText.enabled = false;
        // gamePausedText.enabled = true;
    }

    private void ProvideInstance()
    {
        switch (Instance)
        {
            case null:
                Instance = this;
                break;
            default:
                Debug.LogError("Duplicated Game Manager", gameObject);
                break;
        }
    }

    private void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        SetCanvas(newGameState);
    }

    private void SetCanvas(GameState newGameState)
    {
        InGameCanvas.enabled = newGameState == GameState.GAME;
        pauseMenuCanvas.enabled = newGameState == GameState.PAUSE_MENU;
        GameOverCanvas.enabled = newGameState == GameState.LEVEL_COMPLETED;
    }

    private void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GAME);
    }

    private void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }

    private void GameOver()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }

    public void AddPoints(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void AddKeys(int keys)
    {
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
        if (lives == 0)
        {
            GameOver();
            // gameOverText.enabled = true;
        }
    }

    public void EndGateHit()
    {
        if (keysFound >= 3)
        {
            LevelCompleted();
            // levelCompletedText.enabled = true;
        }
    }

    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OnExitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}