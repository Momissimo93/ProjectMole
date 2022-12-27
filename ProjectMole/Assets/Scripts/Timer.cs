using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManger;

public class Timer : MonoBehaviour
{

    [SerializeField] bool debugMode;

    private float timer;
    private float waves;
    private float breath;
    private bool startCountDown;
    private Queue<Job> jobsQueue;

    public delegate void CountDownFinishd(GameManger.GameState gameState);
    public CountDownFinishd countDownFinish;

    public static Timer instance;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(startCountDown)
        {
            if (timer > 0)
            {

                if(jobsQueue.Count > 0 && jobsQueue.Peek().timeInSeconds == Mathf.Floor(timer))
                {
                    if(debugMode)
                    {
                        Debug.Log("Fire jobs at " + jobsQueue.Peek().timeInSeconds);
                    }
                    GameManger.instance.NewJob(jobsQueue.Dequeue());
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
