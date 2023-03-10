using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    public PlayerFallingState(Player player) : base(player)
    {

    }

    public override void EnterState()
    {
        if (player.animator != null)
        {
            player.animator.SetBool("isFalling", true);
        }
    }

    // Update is called once per frame
    public override void UpdateState()
    {
        if (player.feet.isOnGround)
        {
            if (player.animator != null)
            {
                player.animator.SetBool("landing", true);
            }
            if (player.normalizedInput.NormalizedValue != 0)
            {
                player.ChangeState(new PlayerMoveState(player));
            }
            else if (player.normalizedInput.NormalizedValue == 0)
            {
                player.ChangeState(new PlayerIdleState(player));
            }
        }
    }
    public override void FixedUpdateState()
    {
        player.rb.velocity -= player.vectorGravity * player.fallMultiplier * Time.deltaTime;
        HandleMoveInput();
    }
    private void HandleMoveInput()
    {
        player.rb.velocity = new Vector3(player.normalizedInput.NormalizedValue * player.speed * Time.fixedDeltaTime, player.rb.velocity.y);
        
    }
    public override void ExitState()
    {
        if (player.animator != null)
        {
            player.animator.SetBool("isFalling", false);
        }

        //player.playerInputHandler.CanJump();
    }
}
