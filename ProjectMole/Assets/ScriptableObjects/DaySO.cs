using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "ScriptableObjects/Day", order = 1)]

public class DaySO : ScriptableObject
{
    [field: SerializeField]

    public int dayNumber;

    [field: SerializeField]
    public float dayDuration { get; set; }

    public Job[] jobs;
}
