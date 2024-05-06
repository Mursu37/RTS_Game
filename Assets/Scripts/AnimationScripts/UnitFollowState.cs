using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;
    private Unit unit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        unit = animator.transform.GetComponent<Unit>();
       

        //attackController.SetFollowMaterial();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        attackController.FindHigherPriorityTarget();
        // should unit transition to idle state?
        if (attackController.targetToAttack == null)
        {
            var colliders = Physics.OverlapSphere(animator.transform.position, 10, LayerMask.GetMask("Attackble"), QueryTriggerInteraction.Collide);
            if (colliders.Length > 0)
            {
                attackController.targetToAttack = colliders[0].transform;

            }
            else
            {
                animator.SetBool("isFollowing", false);
            }
        } else
        {
            // if there is no other direct command to move
            if(animator.transform.GetComponent<FixMovement>().isCommandedToMove == false)
            {
                // moving unit towards enemy
                Collider x = attackController.targetToAttack.GetComponent<Collider>();
                agent.SetDestination(x.ClosestPoint(animator.transform.position));
                //animator.transform.LookAt(attackController.targetToAttack);


              //  should unit transition to attack state?
                float distanceFromTarget = Vector3.Distance(x.ClosestPoint(animator.transform.position), animator.transform.position);
                //Debug.Log(distanceFromTarget);

                if ((distanceFromTarget + 0.5f) < unit.attackRange)
                {
                    agent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking", true); // move to attacking state
                }

            }
        }
       

    }
    
}
