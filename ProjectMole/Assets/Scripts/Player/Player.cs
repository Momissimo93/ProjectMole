using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float speed;
    [SerializeField]
    public float jumpForce;
    [SerializeField]
    public LayerMask groundLayerMask;
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public float jumpMultiplier;
    [SerializeField] 
    public float fallMultiplier;
    [SerializeField]
    public Transform attackPoint;
    public Feet feet { get; private set; }
    public PlayerInputHandler playerInputHandler { get; set; }
    public Rigidbody rb { get; private set; }
    public bool canJump { get; set; }
    public bool canAttack { get; set; }
    public bool canRepair { get; set; }
    public Vector3 vectorGravity { get; private set; }

    public bool facingRight = true;

    [SerializeField]
    private float attackRadius;
    [SerializeField]
    private LayerMask targetLayer;

    private CapsuleCollider capsuleCollider;
    private PlayerBaseState currentState;
    private Vector2 currentMoveInput;
    private bool drawAttackingSphere;
    private IEnumerator actionCoroutine;

    public delegate void AttackDelegate();
    public AttackDelegate attackDelegate;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        playerInputHandler = gameObject.GetComponent<PlayerInputHandler>();
        feet = gameObject.GetComponentInChildren<Feet>();
    }

    // Start is called before the first frame update
    void Start()
    {
        vectorGravity = new Vector3(0, -Physics.gravity.y, 0);
        canAttack = true;
        canRepair = true;
        CanJump(); 
        ChangeState(new PlayerIdleState(this));
    }

    public void ChangeState(PlayerBaseState nextState)
    {
        currentState?.ExitState();
        currentState = nextState;
        currentState?.EnterState();
    }

    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
    private void Update()
    {
        currentState.UpdateState();
        attackDelegate?.Invoke();
    }

    public void Attack()
    {
        if(canAttack)
        {
            if (animator != null)
            {
                animator.SetTrigger("attack");
            }
            canAttack = false;
            canRepair = false;
        }
    }
    public void OnAttackAnimationEvent()
    {
       
        attackDelegate += TryInflictDamage;

        drawAttackingSphere = true;
    }
    public void OffAttackAnimationEvent()
    {
        attackDelegate -= TryInflictDamage;

        drawAttackingSphere = false;
        canAttack = true;
        canRepair = true;
    }

    private void TryInflictDamage()
    {
        drawAttackingSphere = true;

        Collider[] colliders;
        colliders = Physics.OverlapSphere(attackPoint.position, attackRadius, targetLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out IDamagable d))
            {
                d.GetDamage();
            }
        }
    }

    public void Repair()
    {
        if (canRepair)
        {
            if (animator != null)
            {
                animator.SetTrigger("repair");
            }
            canAttack = false;
            canRepair = false;
            Debug.Log("Repair");
            actionCoroutine = RepairCoroutine(1f);
            StartCoroutine(actionCoroutine);
        }
    }
    private IEnumerator RepairCoroutine(float s)
    {
        drawAttackingSphere = true;

        Collider[] colliders;
        colliders = Physics.OverlapSphere(transform.position, attackRadius, targetLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out IFixable d))
            {
                d.Fix();
            }
        }

        yield return new WaitForSeconds(s);

        drawAttackingSphere = false;
        canAttack = true;
        canRepair = true;
    }

    public void CanJump() => canJump = true;
    private void OnDrawGizmos()
    {
        if(drawAttackingSphere)
        {
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
    }
}
