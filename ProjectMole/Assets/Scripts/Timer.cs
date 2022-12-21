using System;
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
    Queue <Job> jobsQueue;

    public delegate void CountDownFinishd(GameManger.GameState gameState);
    public CountDownFinishd countDownFinish;
    // Update is called once per frame
    void Update()
    {
        if(startCountDown)
        {
            if (timer > 0)
            {

                if(jobsQueue.Count > 0 && jobsQueue.Peek().timeInSeconds == Mathf.Floor(timer))
                {
                    Debug.Log("Fire jobs at " + jobsQueue.Peek().timeInSeconds);
                    if (jobsQueue.Peek().eventPosition.GetComponent<Marker>())
                    {
                        IMarkable c;
                        jobsQueue.Peek().eventPosition.TryGetComponent<IMarkable>(out c);
                        c.ActivateMarker();
                    }
                    jobsQueue.Dequeue();
                }

                timer -= Time.deltaTime;
                HUDManager.instance.UpdateTimerUI(Mathf.Floor(timer / 60), Mathf.Floor(timer % 60));
            }
            else
            {
                HUDManager.instance.UpdateTimerUI(00f, 00f);
                countDownFinish?.Invoke(GameManger.GameState.EndLevel);
            }
        }
    }

    public void SetTimer(float value, Job[] jobs)
    {
        Job[] tempJob = jobs;
        timer = value;

        Job temp;

        for(int i = 0; i< tempJob.Length; i++)
        {
            for(int j = i+1; j < tempJob.Length;j++)
            {
                if(tempJob[i].timeInSeconds < tempJob[j].timeInSeconds)
                {
                    temp = tempJob[j];
                    tempJob[j] = tempJob[i];
                    tempJob[i] = temp;
                }
            }
        }
        jobsQueue = new Queue<Job>();

        for(int k = 0; k < tempJob.Length; k++)
        {
            jobsQueue.Enqueue(tempJob[k]);
        }

    }

    public void StartCountDown() => startCountDown = true;
}
