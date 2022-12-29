using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(Player player) : base(player)
    {
    }
    public override void EnterState()
    {
        player.rb.velocity = Vector3.zero;
        //player.animator.SetBool("landing", false);
    }
    public override void UpdateState()
    {
        if (player.normalizedInput.NormalizedValue != 0)
        {
            player.ChangeState(new PlayerMoveState(player));
        }
        else if (player.feet.isOnGround && player.playerInputHandler.isJumpPressed)
        {
            player.playerInputHandler.IsJumpPressed();
            player.ChangeState(new PlayerJumpState(player));
        }
    }
}