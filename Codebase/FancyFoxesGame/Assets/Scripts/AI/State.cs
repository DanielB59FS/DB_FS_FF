using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    public  AIController controller;
    public NavMeshAgent agent;
    protected Animator animator;
    protected UseElementalAbility ability1;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<AIController>();
        agent = GetComponent<NavMeshAgent>();
        ability1 = GetComponent<UseElementalAbility>();
    }
    public virtual void Enter(GameObject owner) { 
        CheckForWalking();
    }
    public virtual void Exit(GameObject owner) { }
    public virtual void Execute(GameObject owner) {
        CheckForWalking();
    }
    
    private void CheckForWalking()
    {
        if (agent.hasPath)
        {
            animator.SetBool("isWalking", true);
        } else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
