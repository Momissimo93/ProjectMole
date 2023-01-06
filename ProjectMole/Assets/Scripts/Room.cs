using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] GameObject room;
    [SerializeField] Hole [] holes;

    private void Start()
    {
        Reset();
    }

    public void GenerateHole()
    {
        room.SetActive(false);
        int i = Random.Range(0, holes.Length);
        holes[i].gameObject.SetActive(true);
        holes[i].isBroken = true;
        holes[i].holeFixed += Reset;
    }

    public void Reset()
    {
        GameManager.instance.DeAtivateMarker(this.gameObject);
        room.gameObject.SetActive(true);

        for (int i = 0; i < holes.Length; i++)
        {
            holes[i].ManualEnemyCleaning();
            holes[i].isBroken = false;
            holes[i].gameObject.SetActive(false);
            holes[i].holeFixed -= Reset;
        }
    }
    public bool IsBroken()
    {
        for (int i = 0; i < holes.Length; i++)
        {
            if (holes[i].IsBroken())
            {
                return true;
            }
        }
        return false;
    }
}
