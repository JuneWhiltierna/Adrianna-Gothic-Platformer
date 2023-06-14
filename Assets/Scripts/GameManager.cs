using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    public  GameState currentGameState = GameState.GAME;

    public Canvas InGameCanvas;
    public Canvas PausedCanvas;
    public Canvas GameOverCanvas;
    public TMP_Text scoreText;
    public TMP_Text keysFoundText;
    public TMP_Text enemiesText;
    public TMP_Text levelCompletedText;
    public TMP_Text gameOverText;
    public TMP_Text gamePausedText;
    public Image[] keyTab;
    public Image[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        ProvideInstance();
        SetDefaultCanvas();
        SetDefaultTexts();
        SetDefaultScore();
        SetDefaultKeysFound();
        SetDefaultEnemies();
        SetDefaultHearts();

        keyTab[3].color = Color.gray;
    }
    // Update is called once per frame
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
                    Debug.Log("InGame");
                    InGame();
                    //gamePausedText.enabled = false;
                    break;
                case GameState.GAME:
                    Debug.Log("Pause");
                    PauseMenu();
                    //gamePausedText.enabled = true;
                    break;
                default:
                    InGame();
                    break;
            }
        }
    }
    private void SetDefaultCanvas()
    => SetCanvas(currentGameState);

    private void SetDefaultScore()
    => scoreText.text = score.ToString();

    private void SetDefaultKeysFound()
    => keysFoundText.text = keysFound.ToString();

    private void SetDefaultHearts()
    => hearts[lives].enabled = false;

    private void SetDefaultEnemies()
    => enemiesText.text = enemiesDefeated.ToString();

    private void SetDefaultTexts()
    {
        //levelCompletedText.enabled = false;
        gameOverText.enabled = false;
        gamePausedText.enabled = true;
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
    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        SetCanvas(newGameState);
    }

    private void SetCanvas(GameState newGameState)
    {
        InGameCanvas.enabled = newGameState == GameState.GAME;
        PausedCanvas.enabled = newGameState == GameState.PAUSE_MENU;
        GameOverCanvas.enabled = newGameState == GameState.LEVEL_COMPLETED;
    }

    void PauseMenu()
        => SetGameState(GameState.PAUSE_MENU);

    void InGame()
        => SetGameState(GameState.GAME);

    void LevelCompleted()
        => SetGameState(GameState.LEVEL_COMPLETED);

    void GameOver()
        => SetGameState(GameState.LEVEL_COMPLETED);

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
        //hearts[lives].enabled = true;
        //lives++;
    }

    public void RemoveHeart()
    {
        hearts[lives - 1].enabled = false;
        lives--;

        if (lives == 0)
        {
            GameOver();
            gameOverText.enabled = true;
        }
    }

    public void EndGateHit()
    {
        if (keysFound >= 3)
        {
            LevelCompleted();
            levelCompletedText.enabled = true;
        }
    }
}
