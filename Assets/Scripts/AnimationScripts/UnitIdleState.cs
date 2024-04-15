using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdleState : StateMachineBehaviour
{

    AttackController attackController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();

        attackController.SetIdleMaterial();

        var colliders = Physics.OverlapSphere(animator.transform.position, 10 * 0.2f, LayerMask.GetMask("Attackble"), QueryTriggerInteraction.Collide);
        if (colliders.Length > 0)
        {
            attackController.targetToAttack = colliders[0].transform;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            // target check
        if(attackController.targetToAttack != null) 
        {
            // transition to follow state
            animator.SetBool("isFollowing", true);
        }
    }
}
