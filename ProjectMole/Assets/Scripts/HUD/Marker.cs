using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour, IMarkable
{
    [SerializeField] GameObject marker;
    public void ActivateMarker() => marker.SetActive(true);
    public void DeactivateMarker() => marker.SetActive(false);
}
