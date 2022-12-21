using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "ScriptableObjects/Day", order = 1)]

public class DaySO : ScriptableObject
{
    [field: SerializeField]
    public GameManger.Level dayName { get; set; }

    [field: SerializeField]
    public Holes[] holes;

    [field: SerializeField]
    public Beams [] beams;

    [field: SerializeField]
    public float dayDuration { get; set; }

}

[Serializable]
public class Holes
{
    public float timeInSeconds;
}
[Serializable]
public class Beams
{
    public float timeInSeconds;
}