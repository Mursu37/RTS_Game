using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FixMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public bool isCommandedToMove;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        // Check if the agent is moving or not
        bool isMoving = agent.hasPath && agent.remainingDistance > agent.stoppingDistance;

        animator.SetBool("isMoving", isMoving); 
        if (agent.pathPending == false && (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)) 
        {
            isCommandedToMove = false;
        }
    }

    private void LateUpdate()
    {
    }
}
