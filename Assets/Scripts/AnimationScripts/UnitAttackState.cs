using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;

    public float stopAttackingDistance = 1.2f; // HUOM T�M�N PIT�� OLLA ISOMPI KUIN float attackingDistance UnitFollowState.cs
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();
        attackController.SetAttackMaterial();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // check if we have target and if we didnt give another move command
        if(attackController.targetToAttack != null && animator.transform.GetComponent<UnitMovement>().isCommandedToMove == false)
        {
            // voi olla buginen my�hemmmin animaatioiden kanssa
            LookAtTarget();

            // moving to enemy
            agent.SetDestination(attackController.targetToAttack.position);


            
            // actually perform attack
           

            // should unit still attack
            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);

            if (distanceFromTarget > stopAttackingDistance || attackController.targetToAttack == null)
            {
                
                animator.SetBool("isAttacking", false); // move back to follow state
            }


        }
        
    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
