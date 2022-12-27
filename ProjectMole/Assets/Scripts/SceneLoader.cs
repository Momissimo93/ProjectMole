using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadIntro()
    {
        SceneManager.LoadScene("Intro");
    }
}
