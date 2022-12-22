using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HUDManager : MonoBehaviour
{
    [SerializeField] private bool debugMode;

    public static HUDManager instance;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateTimerUI(float m, float s)
    {
        float minutes = m;
        float seconds = s;
        string currentTime = string.Format("{00:00}:{1:00}", minutes, seconds);
        timerText.text = currentTime;
        if(debugMode)
        {
            Debug.Log(currentTime);
        }
    }
}
