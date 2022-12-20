using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(Player player) : base(player)
    {

    }
    public override void EnterState()
    {
        player.canJump = false;
        player.rb.velocity = new Vector3(player.rb.velocity.x, player.jumpForce);
        //player.rb.AddForce(Vector2.up * player.jumpForce, ForceMode.Impulse);
    }
    public override void FixedUpdateState()
    {
        HandleMoveInput();
    }
    public override void UpdateState()
    {
        if (player.rb.velocity.y < 0)
        {
            player.ChangeState(new PlayerFallingState(player));
        }
    }
    private void HandleMoveInput()
    {
        if (player.playerInputHandler.normalizedInput != 0)
        {
            player.rb.velocity = new Vector2(player.playerInputHandler.normalizedInput * player.speed * Time.fixedDeltaTime, player.rb.velocity.y);
        }
    }
}
