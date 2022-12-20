using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState
{
    protected Player player;

    protected PlayerBaseState(Player player)
    {
        this.player = player;
    }
    public virtual void EnterState() { }
    public virtual void UpdateState() { }
    public virtual void FixedUpdateState() { }
    public virtual void ExitState() { }
    public virtual void StopCoroutine() { }
}
