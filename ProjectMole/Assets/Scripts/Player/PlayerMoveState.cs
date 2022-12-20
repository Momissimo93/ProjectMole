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
        Debug.Log("Entering MoveState");
    }

    public override void UpdateState()
    {
        if (player.playerInputHandler.normalizedInput == 0)
        {
            player.ChangeState(new PlayerIdleState(player));
        }
        else if (player.feet.isOnGround && player.playerInputHandler.jumpInput)
        {
            player.ChangeState(new PlayerJumpState(player));
        }
    }
    public override void FixedUpdateState()
    {
        Vector3 appliedMovementInput = new Vector3(player.playerInputHandler.normalizedInput, 0, 0).normalized;
        player.rb.velocity = appliedMovementInput * player.speed * Time.fixedDeltaTime;
    }
}
