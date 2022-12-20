using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timer;
    bool startCountDown;

    // Update is called once per frame
    void Update()
    {
        if(startCountDown)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                Debug.Log(timer);
            }
        }
    }
    public void SetTime(float value) => timer = value;
    public void StartCountDown() => startCountDown = true;
}
