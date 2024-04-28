using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitMoveState : StateMachineBehaviour
{
    

    UnitMovement unitMovement;
    
    // right click movement animaatiota varten

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        unitMovement = animator.transform.GetComponent<UnitMovement>();
        if (unitMovement == null)
        {
            Debug.LogError("UnitMovement component not found on the animator's GameObject");
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (unitMovement != null && !unitMovement.isCommandedToMove)
        {
            animator.SetBool("isMoving", false);
        }

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }


}
