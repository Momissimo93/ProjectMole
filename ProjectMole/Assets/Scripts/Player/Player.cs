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
    public Feet feet { get; private set; }
    public PlayerInputHandler playerInputHandler { get; set; }
    public Rigidbody rb { get; private set; }
    public bool canJump { get; set; }

    private CapsuleCollider capsuleCollider;
    private PlayerBaseState currentState;
    private Vector2 currentMoveInput;

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
        Debug.Log("Current state " + currentState);
        currentState.UpdateState();
    }

    public void CanJump() => canJump = true;
}
