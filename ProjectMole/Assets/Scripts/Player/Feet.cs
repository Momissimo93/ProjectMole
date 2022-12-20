using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feet : MonoBehaviour
{
    [SerializeField] Player player;
    public bool isOnGround { get; private set; }
    private void Awake()
    {
        player = gameObject.GetComponentInParent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (player)
        {
            if (1 << other.gameObject.layer == player.groundLayerMask.value)
            {
                isOnGround = true;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (player)
        {
            if (1 << other.gameObject.layer == player.groundLayerMask.value && isOnGround == false)
            {
                isOnGround = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (player)
        {
            if (1 << other.gameObject.layer == player.groundLayerMask.value)
            {
                isOnGround = false;
            }
        }
    }
}