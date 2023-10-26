using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RallyState : State
{
    [SerializeField]  Dictionary<GameObject, int> idleEnemyGroups = new Dictionary<GameObject, int>(); //A dictionary of idle enemies and how many enemies are near each of them.
    GameObject target;
    GameObject mostValuableAlly; //The ally with the most nearby allies.
    [SerializeField] List<GameObject> alliesInRallyRange = new List<GameObject>();
    float originalSpeed;

    // Start is called before the first frame update
    public override void Enter(GameObject owner)
    {
        originalSpeed = agent.speed;
        agent.speed += 6;
        System.Random rng = new System.Random();
        int checkLimit = controller.GetIntelligence() * 10; //percentage intelligence check for choosing the best rally target
        int result = rng.Next(0,100);

        foreach (var enemy in GameManager.Instance.Enemies) //For enemy in the idle state that aren't this, add it to the list.
        {
            if (!idleEnemyGroups.ContainsKey(enemy) && enemy != this.gameObject && enemy.GetComponent<IdleState>())
            {
               idleEnemyGroups.Add(enemy,enemy.GetComponent<AIController>().GetAllyCount());
                if (mostValuableAlly == null)  //Keep track of the idle ally, with the most nearby allies, they are more valuable because more total AI will rally with them.
                {
                    mostValuableAlly = enemy;
                } else if (mostValuableAlly.GetComponent<AIController>().GetAllyCount() < enemy.GetComponent<AIController>().GetAllyCount())
                {
                    mostValuableAlly = enemy;
                }
            }
        }


        
        if (result < checkLimit) //If you pass the intelligence check, you pick the ally with the most nearby allies to rally.
        {
            
            target = mostValuableAlly;
        } else                  //otherwise, you pick a random ally to rally.
        {
            int indexLimit = rng.Next(idleEnemyGroups.Count);
            int index = 0;
            foreach (var ally in idleEnemyGroups)
            {
                if (index >= indexLimit)
                {
                    target = ally.Key;
                    break;
                } else
                {
                    index++;
                }
            }
        }
        base.Enter(owner);

    }

    public override void Execute(GameObject owner)
    {
        int arrivalDistanceOffset = 3; //offset units before AI counts as arrived to it's destination.

        if (agent.destination == GameManager.Instance.PlayerObject.transform.position) //clear the path, so you can target an ally w/o issue.
        {
            agent.ResetPath();
        }
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
        } else
        {
            controller.ChangeState(AIController.States.Idle); //If we cant find a target, go back to idle state, crash protection code.
        }
        
        if (agent.remainingDistance - arrivalDistanceOffset <= 1) //When you arrive at your destination for rally, rally anyone in range.
        {
            //Play rally animation here during polish week
            Rally();
            controller.hasRallied = true;
        }
        base.Execute(owner);
    }

    public override void Exit(GameObject owner)
    {
        agent.SetDestination(GameManager.Instance.PlayerObject.transform.position); //since rallying AI are faster and have different targets, reset the target and speed on exit
        agent.speed = originalSpeed;
        base.Exit(owner);
    }

    private void Rally() //When you are able to rally, rally all the nearby allies in the rally range switching them to Hostile and calling Onrally on them to grant them bonuses.
    {
        foreach (var ally in alliesInRallyRange)
        {
           AIController allyController = ally.GetComponent<AIController>();
           allyController.SetTarget(GameManager.Instance.PlayerObject);
           controller.SetTarget(GameManager.Instance.PlayerObject);
            agent.SetDestination(target.transform.position);
           allyController.ChangeState(AIController.States.Hostile);
            //allyController.OnRally();
            //controller.OnRally();
            controller.ChangeState(AIController.States.Hostile); //dont forget to switch yourself to hostile
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Monster" && !alliesInRallyRange.Contains(other.gameObject))
        {
            alliesInRallyRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Monster" && alliesInRallyRange.Contains(other.gameObject))
        {
            alliesInRallyRange.Remove(other.gameObject);
        }
    }
}

