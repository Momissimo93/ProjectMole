using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public PlayerMoveState(Player player) : base(player)
    {
    }

    public override void EnterState()
    {
        //player.animator.SetBool("landing", false); 

    }
    public override void UpdateState()
    {
        if (player.normalizedInput.NormalizedValue == 0)
        {
            player.ChangeState(new PlayerIdleState(player));
        }
        else if (player.feet.isOnGround && player.playerInputHandler.isJumpPressed)
        {
            player.playerInputHandler.IsJumpPressed();
            player.ChangeState(new PlayerJumpState(player));
        }
        else if (!player.feet.isOnGround)
        {
            player.ChangeState(new PlayerFallingState(player));
        }
    }
    public override void FixedUpdateState()
    {
        player.rb.velocity = new Vector3(player.normalizedInput.NormalizedValue * player.speed * Time.deltaTime, player.rb.velocity.y);
    }
}
