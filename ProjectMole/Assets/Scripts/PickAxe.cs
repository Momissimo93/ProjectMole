using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class PickAxe : MonoBehaviour
{
    [SerializeField] 
    float rotationSpeed = 400f;

    [SerializeField] 
    float moveSpeed = 400f;

    [SerializeField]
    float attackRadius;

    [SerializeField]
    float offset;

    [SerializeField]
    private float threshold;

    [SerializeField]     
    LayerMask playerLayer;

    [SerializeField] 
    LayerMask boundariesLayer;

    [SerializeField]
    LayerMask enemyLayer;

    [SerializeField]
    LayerMask fixableLayer;

    private Vector3 initialPosition;
    private float zRotation;
    private Vector3 direction;
    private Player player;
    private bool isOverTheThreshold;
    private bool hasCollided;

    public delegate void AttackDelegate(Vector3 p, float r, LayerMask l);
    public AttackDelegate onAttack;
    public delegate void FixDelegate(Vector3 p, float r, LayerMask l);
    public FixDelegate onFix;

    public UnityEvent onAttackComplete;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        initialPosition = transform.position;
        isOverTheThreshold = false;
        hasCollided = false;

        onAttack += Attack;
        onFix += Fix;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOverTheThreshold)
        {
            IsOverTheThreshold();
        }
        if (!hasCollided)
        {
            HasCollided();
        }

        zRotation = rotationSpeed * Time.deltaTime;
        transform.Rotate(0, 0, zRotation);
        if(hasCollided || isOverTheThreshold)
        {
            MoveTowardsPlayer();
        }
        else
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        onAttack?.Invoke(transform.position + transform.up * offset, attackRadius, enemyLayer);
        onFix?.Invoke(transform.position + transform.up * offset, attackRadius, fixableLayer);
    }

    void MoveTowardsPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.attackPoint.transform.position, moveSpeed * Time.deltaTime);
        Collider[] hit = Physics.OverlapSphere(transform.position, 1, playerLayer);
        if(hit.Length > 0)
        {

            onAttackComplete.Invoke();
            onAttackComplete.RemoveAllListeners();
            Destroy(gameObject); 
        }
    }

    void HasCollided()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.4f, boundariesLayer);
        if (hitColliders.Length > 0)
        {
            hasCollided = true;
        }
    }
    void IsOverTheThreshold()
    {
        Vector3 distance = transform.position - initialPosition;
        float distanceMagnitude = distance.magnitude;
        if (distanceMagnitude >= threshold)
        {
            isOverTheThreshold = true;
        }
    }
    public void Attack(Vector3 position, float collision_Radius, LayerMask layerMask)
    {
        //Debug.Log("Attack");
        Collider[] colliders;
        colliders = Physics.OverlapSphere(position, collision_Radius, layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out IDamagable d))
            {
                d.GetDamage();
                hasCollided = true;
            }
        }
    }
    public void Fix(Vector3 position, float collision_Radius, LayerMask layerMask)
    {
        //Debug.Log("Attack");
        Collider[] colliders;
        colliders = Physics.OverlapSphere(position, collision_Radius, layerMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out IFixable f))
            {
                f.Fix();
                hasCollided = true;
            }
        }
    }

    public void SetDirection(Vector3 dir) => direction = dir;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position +transform.up * offset, attackRadius);
    }
}
