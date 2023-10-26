using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    // Start is called before the first frame update
    public override void Enter(GameObject owner)
    {
        base.Enter(owner);
        animator.SetBool("isIdle", true);
    }

    public override void Execute(GameObject owner)
    {
        base.Execute(owner);
        //Switch to idle animation here.
    }

    public override void Exit(GameObject owner)
    {
        base.Exit(owner);
        animator.SetBool("isIdle", false);
    }
}
