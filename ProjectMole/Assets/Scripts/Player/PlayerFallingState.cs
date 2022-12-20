using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(Player player) : base(player)
    {

    }

    // Update is called once per frame
    public override void UpdateState()
    {
        if (player.feet.isOnGround)
        {
            if (player.playerInputHandler.normalizedInput != 0)
            {
                player.ChangeState(new PlayerMoveState(player));
            }
            else if (player.playerInputHandler.normalizedInput == 0)
            {
                player.ChangeState(new PlayerIdleState(player));
            }
        }
    }
    public override void FixedUpdateState()
    {
        HandleMoveInput();
    }
    private void HandleMoveInput()
    {
        if (player.playerInputHandler.normalizedInput != 0)
        {
            player.rb.velocity = new Vector2(player.playerInputHandler.normalizedInput * player.speed * Time.fixedDeltaTime, player.rb.velocity.y);
        }
    }
    public override void ExitState()
    {
        //player.playerInputHandler.CanJump();
    }
}
