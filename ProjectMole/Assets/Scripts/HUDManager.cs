using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[RequireComponent(typeof(Timer))]
public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    public Timer timer;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        timer = gameObject.GetComponent<Timer>();
    }

}
