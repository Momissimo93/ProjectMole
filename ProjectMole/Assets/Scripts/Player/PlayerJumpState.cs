using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(Player player) : base(player)
    {

    }
    public override void EnterState()
    {
        if(player.animator != null)
        {
            player.animator.SetBool("landing", false);
            player.animator.SetBool("isJumping", true);
        }
        player.canJump = false;
        player.rb.velocity = new Vector3(player.rb.velocity.x, player.jumpForce, 0);
    }
    public override void FixedUpdateState()
    {
        HandleMoveInput();
    }
    public override void UpdateState()
    {
        player.rb.velocity += player.jumpMultiplier * player.vectorGravity * Time.deltaTime;
        if (player.rb.velocity.y < 0)
        {
            player.ChangeState(new PlayerFallingState(player));
        }
    }
    private void HandleMoveInput()
    {
        player.rb.velocity = new Vector3(player.playerInputHandler.normalizedInput * player.speed * Time.fixedDeltaTime, player.rb.velocity.y); //this was previosuly used
    }
    public override void ExitState()
    {
        if (player.animator != null)
        {
            player.animator.SetBool("isJumping", false);
        }
    }
}
