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
            player.animator.SetBool("isJumping", true);
            player.animator.SetBool("landing", false);
        }
        player.canJump = false;
        // player.rb.velocity = new Vector3(player.rb.velocity.x, player.jumpForce);
        player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode.Impulse);
    }
    public override void FixedUpdateState()
    {
        HandleMoveInput();

    }
    public override void UpdateState()
    {
        //player.rb.velocity =  new Vector2(player.playerInputHandler.normalizedInput * player.rb.velocity.x * player.speed , player.rb.velocity.y);
        if (player.rb.velocity.y < 0)
        {
            Debug.Log("Velocity < 0");
            player.ChangeState(new PlayerFallingState(player));
        }
    }
    private void HandleMoveInput()
    {
        player.rb.velocity = new Vector3(player.playerInputHandler.normalizedInput * player.speed * Time.fixedDeltaTime, player.rb.velocity.y);
        //player.rb.velocity = new Vector2(player.playerInputHandler.normalizedInput * player.speed * Time.fixedDeltaTime, player.rb.velocity.y);

    }
    public override void ExitState()
    {
        if (player.animator != null)
        {
            player.animator.SetBool("isJumping", false);
        }
    }
}
