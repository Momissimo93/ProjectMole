using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManger;

public class Timer : MonoBehaviour
{
    float timer;
    float waves;
    float breath;
    bool startCountDown;

    public delegate void CountDownFinishd(GameManger.GameState gameState);
    public CountDownFinishd countDownFinish;
    // Update is called once per frame
    void Update()
    {
        if(startCountDown)
        {
            if (timer > 0)
            {
  
                timer -= Time.deltaTime;
                float minutes = Mathf.Floor(timer / 60);
                float seconds = Mathf.Floor(timer % 60);
                string currentTime = string.Format("{00:00}:{1:00}", minutes, seconds);
                HUDManager.instance.timerText.text = currentTime;
                Debug.Log(currentTime);
            }
            else
            {
                countDownFinish?.Invoke(GameManger.GameState.EndLevel);

            }
        }
    }
    public void SetTime(float value) => timer = value;
    public void StartCountDown() => startCountDown = true;
}
