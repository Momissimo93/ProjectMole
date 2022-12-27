using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    BoxCollider c;

    [SerializeField]
    GameObject destination;
    private void Awake()
    {
        c = gameObject.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            Debug.Log("Player");
            other.transform.position = destination.transform.position;
        }
    }
}

