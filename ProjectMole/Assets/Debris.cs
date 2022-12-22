using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void PlayAniamtionDebris()
    {
        animator.Play("DebrisFalling");
    }
}
