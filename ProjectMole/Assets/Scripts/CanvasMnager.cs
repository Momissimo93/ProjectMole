using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManger;

public class CanvasMnager : MonoBehaviour
{
    public static CanvasMnager instance;

    [SerializeField]
    GameObject GameOverCanvas;
    [SerializeField]
    GameObject JobDoneMenu;
    [SerializeField]
    GameObject PauseMenu;

    private void Awake()
    {
        instance = this;
    }
    public void OnMainMenu()
    {
        SceneLoader.instance.LoadIntro();
        Time.timeScale = 1f;
    }
    public void OnResume()
    {
        PauseMenu.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnNextDay()
    {
        JobDoneMenu.gameObject.SetActive(false);
        GameManger.instance.UpdateGameState(GameState.SetDifficulty);
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        PauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void JobDone()
    {
        JobDoneMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void GameOver()
    {
        GameOverCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}