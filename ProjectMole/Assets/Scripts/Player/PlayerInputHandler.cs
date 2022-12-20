using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInputActions playerInputActions;
    public Vector2 rawMovementInput { get; private set; }
    public int normalizedInput;
    public bool jumpInput = false;
    void Start()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        playerInputActions.Player.Move.started += ctx => OnMoveInput(ctx);
        playerInputActions.Player.Move.performed += ctx => OnMoveInput(ctx);
        playerInputActions.Player.Move.canceled += ctx => OnMoveInput(ctx);

        playerInputActions.Player.Jump.started += ctx => OnJumpInput(ctx);
        playerInputActions.Player.Jump.performed += ctx => OnJumpInput(ctx);
        playerInputActions.Player.Jump.canceled += ctx => OnJumpInput(ctx);
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {

        rawMovementInput = context.ReadValue<Vector2>();
        normalizedInput = (int)(rawMovementInput * Vector2.right).normalized.x;

        if (context.started)
        {

            rawMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.performed)
        {

            rawMovementInput = context.ReadValue<Vector2>();

        }
        else if (context.canceled)
        {
            rawMovementInput = context.ReadValue<Vector2>();
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump");
            jumpInput = true;
        }
        else if (context.canceled)
        {
            jumpInput = false;
        }
    }

    public void Isjumping() => jumpInput = false;
}
