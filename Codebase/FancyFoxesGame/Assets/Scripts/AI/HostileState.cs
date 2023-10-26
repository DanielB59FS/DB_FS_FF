using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using System;
public class HostileState : State
{

    bool fired = false;
    float dodgeMod;
    Vector3 dir;

    public override void Enter(GameObject owner) 
    {
        dodgeMod = (Mathf.Pow(controller.GetIntelligence(), 2) / 3) + 10; //forumla based on creatures intelligence for how often it dodges

        if (GameManager.Instance.PlayerObject) //Set-up navigation to the player
        {
            controller.SetTarget(GameManager.Instance.PlayerObject);
            agent.SetDestination(controller.target.transform.position);
        } else
        {
            controller.ChangeState(AIController.States.Idle);
        }
        base.Enter(owner);

    }

    public override void Execute(GameObject owner)
    {
        base.Execute(owner);
        CheckForOncomingAbility(); 
        
   
        agent.SetDestination(controller.target.transform.position);
        if (agent.remainingDistance < controller.GetRange()) 
        {
            //Enter Attack Logic Below
            if (controller.GetMana() > 0 && !fired) //Add resources that AI can have decision making on what to spend later, decision making code will be added in polish clash.
            {
                StartCoroutine(UseAbility(ability1, ability1.ability.abilityBaseCooldown));
                fired = true;
            }
        }
        
     
    }

    public IEnumerator UseAbility(UseElementalAbility abilityToUse, float cooldown) //code to test functionality, will implement better solutin later using mana and proper cooldown code.
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldown);
            animator.SetTrigger("usedAbility");
            abilityToUse.AlternateUseAbility(ability1.spawnPoint, this.GetComponent<SphereCollider>());
        }
    }

    public override void Exit(GameObject owner)
    {
        base.Exit(owner);
    }

    void CheckForOncomingAbility()
    {
        RaycastHit hit;
        float dodgeDistance = GetComponent<BoxCollider>().size.x * 1.65f; //How far the AI will dodge in the given direction
        Vector3 heightOffset = new Vector3(0, 2, 0); //moves the target up off the ground
        int targetLayerId = 2;
        int timeToDespawnSmoke = 1;

        if (Physics.SphereCast(this.transform.position + new Vector3(0, 0.65f, 0), GetComponent<BoxCollider>().size.x / 2, transform.forward, out hit, 3))
            {
            if (hit.collider.gameObject.CompareTag("Ability") && hit.collider.gameObject.layer != targetLayerId)
            {
                hit.collider.gameObject.layer = targetLayerId; //Prevent further detection by moving projectile to ignoring layer.
                int random = Random.Range(0, 100);
                
                if (random < dodgeMod) //If dodge is successful, dodge either left or right and spawn smoke at start and destination locations.
                {
                    dodgeMod -= 10; //Decrease dodge chance by 10% for each successful dodge until it fails.
                    
                    random = Random.Range(0, 100); //rng number for determining if AI will dodge left or right.
                    var puff1 = Instantiate(controller.smokePuff, this.transform.position,this.transform.rotation); //smoke puff at current location
                    if (random <= 49)
                    {
                        this.transform.position = transform.position + (transform.right * dodgeDistance);
                    } else
                    {
                        this.transform.position = transform.position + (-transform.right * dodgeDistance);
                    }
                    var puff2 = Instantiate(controller.smokePuff, this.transform.position + heightOffset, this.transform.rotation); //smoke puff at target location
                    Destroy(puff1, timeToDespawnSmoke); //Destroy both smoke puffs.
                    Destroy(puff2, timeToDespawnSmoke);
                    
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, GetComponent<BoxCollider>().size.x/2);
        Gizmos.DrawLine(transform.position, dir);
    }
}
