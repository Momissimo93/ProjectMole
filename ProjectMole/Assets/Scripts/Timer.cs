using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] bool debugMode;

    private float timer;
    private bool startCountDown;
    private Queue<Job> jobsQueue;

    public delegate void CountDownFinishd(GameManager.GameState gameState);
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
                    GameManager.instance.NewJob(jobsQueue.Dequeue());
                }

                timer -= Time.deltaTime;
                HUDManager.instance.UpdateTimerUI(Mathf.Floor(timer / 60), Mathf.Floor(timer % 60));
            }
            else
            {
                HUDManager.instance.UpdateTimerUI(00f, 00f);
                countDownFinish?.Invoke(GameManager.GameState.EndLevel);
            }
        }
    }

    public void SetTimer(float value, Job[] jobs)
    {
        Job[] tempJob = jobs;
        timer = value;

        Job temp;

        //Ordering the jobs according to their timing 
        for(int i = 0; i < tempJob.Length; i++)
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
