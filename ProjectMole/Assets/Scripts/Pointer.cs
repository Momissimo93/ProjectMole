using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    private Vector3 target;
    private Vector3 difference;

    private NormalizedInput normalizedInput;

    public static Pointer instance;

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        Cursor.visible =false;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        normalizedInput = new NormalizedInput();
    }

    void Update()
    {
        target = mainCamera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        this.transform.position = target;
        difference = target - Player.instance.transform.position;

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
   
        if(rotationZ >= -90 && rotationZ <= 90)
        {
            Debug.Log("facing right");
            Player.instance.Flip(1);
        }
        else
        {
            Debug.Log("facing left");
            Player.instance.Flip(-1);
        }
        
    }
    public Vector3 Direction()
    {
        float distance = difference.magnitude;
        Vector2 direction = difference / distance;
        direction.Normalize();
        return direction;
    }
}
