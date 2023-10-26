using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class FleeState : State
{
    GameObject[] exits;
    [SerializeField] GameObject fleeTarget; //Storing flee target just in case we want to make the flee target moving in the future

    public override void Enter(GameObject owner) //On enter get a list of possible exits, pick one at random and start going towards it.
    {
        
        exits = GameObject.FindGameObjectsWithTag("Exit");
        agent.speed *= 3.5f;
        if (exits.Length == 0) //If the scene hasn't set up exits, we will go to idle state, crash protection code.
        {
            controller.ChangeState(AIController.States.Idle);
            return;
        }
        LookForExit();
        base.Enter(owner);
        
    }

    public override void Execute(GameObject owner)
    {
        base.Execute(owner);
        float despawnDistance = 5.0f;
        if (Vector3.Distance(fleeTarget.transform.position, this.gameObject.transform.position) < despawnDistance) //If you reach the exit, you despawn and player doesn't get kill credit
        {
            Destroy(this.gameObject);
        }

    }

    private void LookForExit()
    {
        float closestDistance = Vector3.Distance(agent.transform.position,exits[0].transform.position);
        System.Random rng = new System.Random();

        if ( rng.Next(100) < controller.GetIntelligence() * 10) //If you pass the intelligence check, you pick the closest exit to you, otherwise you pick a random exit.
        {
            foreach (GameObject exit in exits)
            {
                if (Vector3.Distance(agent.transform.position, exit.transform.position) < closestDistance)
                {
                    closestDistance = Vector3.Distance(agent.transform.position, exit.transform.position);
                    fleeTarget = exit;
                }
            }
        } 
        if (fleeTarget == null){
            fleeTarget = exits[rng.Next(exits.Length)];
        }

        agent.SetDestination(fleeTarget.transform.position);
    }
}
