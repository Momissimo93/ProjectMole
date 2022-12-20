using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Day", menuName = "ScriptableObjects/Day", order = 1)]

public class DaySO : ScriptableObject
{
    [field: SerializeField]
    public GameManger.Level dayName { get; set; }

    [field: SerializeField]
    public int numberOfHoles { get; set; }

    [field: SerializeField]
    public int numberOfBeams { get; set; }

    [field: SerializeField]
    public float delta { get; set; }

    [field: SerializeField]
    public float dayDuration { get; set; }

}
