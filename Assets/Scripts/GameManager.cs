using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GameState
{
    [InspectorName("Gameplay")] GAME,
    [InspectorName("Pause")] PAUSE_MENU,
    [InspectorName("Level completed")] LEVEL_COMPLETED
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public  GameState currentGameState = GameState.PAUSE_MENU;

    public Canvas InGameCanvas;
    public Canvas PausedCanvas;
    public Canvas GameOverCanvas;
    public TMP_Text scoreText;
    public TMP_Text enemiesText;
    public TMP_Text levelCompletedText;
    public TMP_Text gameOverText;
    public TMP_Text gamePausedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        ProvideInstance();
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (isPaused)
        //    {
        //        InGame();
        //    }
        //    else
        //    {
        //        PauseMenu();
        //    }
        //}
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
                    gamePausedText.enabled = false;
                    break;
                case GameState.GAME:
                    PauseMenu();
                    gamePausedText.enabled = true;
                    break;
                default:
                    InGame();
                    break;
            }
        }
    }
    private void SetDefaultCanvas()
    => SetCanvas(currentGameState);

    private void SetDefaultTexts()
    {
        levelCompletedText.enabled = false;
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

}
