using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class UnitMoveState : StateMachineBehaviour
{
    

    UnitMovement unitMovement;
    private NavMeshAgent agent;
    AttackController attackController;
    

    // right click movement animaatiota varten

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        agent = animator.transform.GetComponent<NavMeshAgent>();
        unitMovement = animator.transform.GetComponent<UnitMovement>();
        if (unitMovement == null)
        {
            Debug.LogError("UnitMovement component not found on the animator's GameObject");
        }
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the animator's GameObject");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log(animator.GetBool("isMoving"));
        if (agent == null) return;

        if (agent != null && (agent.remainingDistance <= agent.stoppingDistance && !agent.hasPath))
        {
            animator.SetBool("isMoving", false);
        }


        if (unitMovement != null && !unitMovement.isCommandedToMove)
        {
            animator.SetBool("isMoving", false);
            //Debug.Log("test");
        }

        //Debug.Log(animator.GetBool("isMoving"));
    }

    

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("test2");
    }


}
