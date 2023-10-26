using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
   
    public override void Enter(GameObject owner)
    {
        base.Enter(owner);
        agent.isStopped = true;
        animator.SetTrigger("isDead");
    }

    public override void Exit(GameObject owner)
    {
        base.Enter(owner);
        Destroy(owner.gameObject); 
    }

    public override void Execute(GameObject owner)
    {
        base.Enter(owner);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > animator.GetCurrentAnimatorStateInfo(0).length) //If the animation has been playing longer than it's duration than exit this state, its done
            {
                var puff = Instantiate(controller.smokePuff, this.transform.position, this.transform.rotation);
                int random = UnityEngine.Random.Range(0, 100);
                if (random <= controller.powerDropRate)
                {
                    Instantiate(controller.powerUp, this.transform.position, this.transform.rotation);
                }
                Destroy(puff.gameObject, 0.5f);
                Exit(owner); //Since death state doesn't repeat, immediately exit after first execution
            }
        }
       
    }
}
