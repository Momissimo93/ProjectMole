using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Hole;

public class Beam : MonoBehaviour, IFixable
{
    public bool isAnActiveHole { get; set; }
    [SerializeField] 
    Animator animator;
    [SerializeField]
    Collider sphereCollider;

    public void Fix()
    {
        animator.SetTrigger("Fix");
        sphereCollider.enabled = false;
        isAnActiveHole = false;
    }
    public void Break()
    {
        sphereCollider.enabled = true;
        isAnActiveHole = true;
        animator.SetTrigger("Break");
    }
    public void Reset()
    {
        if (isAnActiveHole)
        {
            sphereCollider.enabled = false;
            animator.SetTrigger("Fix");
            isAnActiveHole = false;
        }
    }
    public bool IsAnActiveHole() => isAnActiveHole;
}