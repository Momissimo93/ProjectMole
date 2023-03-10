using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.UI.Toggle;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInputActions playerInputActions;

    private Player player;

    public Vector2 rawMovementInput { get; private set; }
    //public int normalizedInput;
    public bool canJump = true;
    public bool isJumpPressed;

    public static PlayerInputHandler instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {       
        playerInputActions = new PlayerInputActions();
        player = gameObject.GetComponent<Player>();
        playerInputActions.Enable();

        playerInputActions.Player.Move.started += ctx => OnMoveInput(ctx);
        playerInputActions.Player.Move.performed += ctx => OnMoveInput(ctx);
        playerInputActions.Player.Move.canceled += ctx => OnMoveInput(ctx);

        playerInputActions.Player.Jump.started += ctx => OnJumpInput(ctx);
        playerInputActions.Player.Jump.performed += ctx => OnJumpInput(ctx);
        playerInputActions.Player.Jump.canceled += ctx => OnJumpInput(ctx);

        playerInputActions.Player.Attack.started += ctx => OnAttack(ctx);
        playerInputActions.Player.Attack.performed += ctx => OnAttack(ctx);
        playerInputActions.Player.Attack.canceled += ctx => OnAttack(ctx);

        playerInputActions.Player.Repair.started += ctx => OnRepair(ctx);
        playerInputActions.Player.Repair.performed += ctx => OnRepair(ctx);
        playerInputActions.Player.Repair.canceled += ctx => OnRepair(ctx);

        playerInputActions.Player.Pause.started += ctx => OnPause(ctx);

        playerInputActions.Player.Throw.started += ctx => OnThrow(ctx);

        playerInputActions.Player.PointUp.started += ctx => OnPointingUp(ctx);
        playerInputActions.Player.PointUp.canceled += ctx => OnPointingUp(ctx);

        playerInputActions.Player.Click.started += ctx => OnThrow(ctx);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        rawMovementInput = context.ReadValue<Vector2>();
        player.normalizedInput.NormalizedValue = (int)(rawMovementInput * Vector2.right).normalized.x;
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {

        if(context.started)
        {
            isJumpPressed = true;
        }
        else if (context.canceled)
        {
            isJumpPressed = false;
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        player.Attack();
    }

    public void OnRepair(InputAction.CallbackContext context)
    {
        player.Repair();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        CanvasManager.instance.PauseGame();
    }

    public void OnPointingUp(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            player.isPointingUp = true;
        }
        else if (context.canceled)
        {
            player.isPointingUp = false;
        }
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        player.Throw();
    }
    public void IsJumpPressed() => isJumpPressed = false;
}
