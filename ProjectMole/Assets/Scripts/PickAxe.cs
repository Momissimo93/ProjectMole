using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAxe : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 400f;
    [SerializeField] float moveSpeed = 400f;
    float zRotation;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        zRotation = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, zRotation);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void SetDirection(Vector3 dir) => direction = dir;
}
