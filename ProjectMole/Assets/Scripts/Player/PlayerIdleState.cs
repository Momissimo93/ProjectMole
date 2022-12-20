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
        Debug.Log("Entering IdleState");
        player.rb.velocity = Vector3.zero;
    }
    public override void UpdateState()
    {
        if (player.playerInputHandler.normalizedInput != 0)
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