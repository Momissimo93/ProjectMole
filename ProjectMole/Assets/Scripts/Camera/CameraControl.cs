using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private GameObject mainChar;
    [SerializeField]
    private float interpolationSpeed = 5f;
    [SerializeField]
    private Vector2 offset;
    [SerializeField]
    private Player player;

    private float verExtent;
    private float horExtent;
    private float leftB;
    private float rightB;
    private float topB;
    private float bottomB;
    private Camera cam;
    private Bounds sceneBounds;

    void Start()
    {
        cam = GetComponent<Camera>();
        player = FindObjectOfType<Player>();
        Collider[] sceneColliders2D = FindObjectsOfType<Collider>();

        foreach (Collider coll in sceneColliders2D)
        {
            sceneBounds.Encapsulate(coll.bounds);
        }

        GetExtents();
        GetBounds();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(Mathf.Clamp(player.transform.position.x, leftB, rightB) + offset.x, Mathf.Clamp(player.transform.position.y, bottomB, topB) + offset.y, transform.position.z), Time.deltaTime * interpolationSpeed);
    }

    void GetExtents()
    {
        if (GetComponent<Camera>())
        {
            verExtent = GetComponent<Camera>().orthographicSize;
            horExtent = verExtent * GetComponent<Camera>().aspect;
        }
    }

    void GetBounds()
    {
        if (GetComponent<Camera>())
        {

            leftB = sceneBounds.min.x + horExtent - offset.x;
            rightB = sceneBounds.max.x - horExtent - offset.x;

            bottomB = sceneBounds.min.y + verExtent - offset.y;
            topB = sceneBounds.max.y - verExtent - offset.y;
        }
    }
}
