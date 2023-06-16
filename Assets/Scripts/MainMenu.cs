using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private IEnumerator LoadYourAsyncScene(string levelName)
    {
        var asyncLoad = SceneManager.LoadSceneAsync(levelName);
        while (!asyncLoad.isDone) yield return null;
    }

    public void OnLevel1ButtonPressed()
    {
        StartCoroutine(LoadYourAsyncScene("Level 1"));
    }

    public void OnLevel2ButtonPressed()
    {
        StartCoroutine(LoadYourAsyncScene("Level 2"));
    }


    public void OnExitButtonPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}