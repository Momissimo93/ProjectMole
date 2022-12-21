using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsManager : MonoBehaviour
{
    [SerializeField] GameObject room;
    [SerializeField] Hole[] holes;

    private void Start()
    {
        Reset();
    }

    public void GenerateHole()
    {
        room.SetActive(false);
        int i = Random.Range(0,holes.Length);
        holes[i].gameObject.SetActive(true);
    }

    public void Reset()
    {
        room.gameObject.SetActive(true);
        for(int i = 0; i < holes.Length; i++)
        {
            holes[i].gameObject.SetActive(false);
        }
    }
}
