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
        player.rb.velocity = new Vector3(player.rb.velocity.x, player.jumpForce);
    }
    // Start is called before the first frame update
    public override void FixedUpdateState()
    {

    }
    public override void UpdateState()
    {
        if (player.rb.velocity.y < 0)
        {
            Debug.Log("Falling");
        }
    }
}
