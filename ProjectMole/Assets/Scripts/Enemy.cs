using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float speed;
    [SerializeField]
    private ParticleSystem hitParticleSystem;
    [SerializeField]
    private float destroctionTrasholdValue;

    public delegate void EnemyDestroyDelegate();
    public EnemyDestroyDelegate enemyDestroyDelegate;

    public delegate void HoleCanbeFixedDelegate();
    public HoleCanbeFixedDelegate holeCanBeFixed;

    private bool hit;
    private Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position;
        animator = gameObject.GetComponent<Animator>();
        hit = false;
    }
    public void GetDamage()
    { 
        if(!hit)
        {
            hit = true;
            Debug.Log("Damage");
            holeCanBeFixed?.Invoke();
            animator.enabled = false;
            StartCoroutine(Flash());
            hitParticleSystem.Play();
        }
    }
    IEnumerator Flash()
    {
        spriteRenderer.color = new Color(1f,1f,1f,0f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 255f);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(Flash());
    }
    private void Update()
    {
        if(hit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
        }
        if (transform.position.y > startingPos.y + destroctionTrasholdValue)
        {
            enemyDestroyDelegate?.Invoke();

        }
    }
}
