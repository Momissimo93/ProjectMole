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

        //player.transform.Translate(player.playerInputHandler.rawMovementInput * Time.deltaTime * player.speed); 1)
        //player.rb.velocity = player.playerInputHandler.rawMovementInput * player.speed; //2)

        if (player.playerInputHandler.normalizedInput == 0)
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
        Vector3 appliedMovementInput = new Vector3(player.playerInputHandler.normalizedInput, player.rb.velocity.y).normalized; 
        player.rb.velocity = appliedMovementInput * player.speed * Time.fixedDeltaTime;
    }
}
