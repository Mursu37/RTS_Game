using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;

    // debug yms kunnes tulevat oikeat modelit
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;
    //   public int unitDamage; // turha atm
    private Unit unit;

    public int currentTargetPriorityValue = 1;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack = null;
        }
    }
    
    public Collider FindHighestPriority(Collider[] targets)
    {
        Collider highestPriorityTarget = targets[0];
        var currentHighestPriorityValue = 1;

        foreach (var collider in targets)
        {
            var priority = collider.GetComponent<IPriority>();
            if (priority != null)
            {
                if (priority.Priority > currentHighestPriorityValue)
                {
                    highestPriorityTarget = collider;
                    currentHighestPriorityValue = priority.Priority;
                }
            }
        }

        return currentHighestPriorityValue > currentTargetPriorityValue ? highestPriorityTarget : targetToAttack.GetComponent<Collider>();
    }

    public bool FindHigherPriorityTarget()
    {
        Collider target;
        if (targetToAttack == null)
        {
            currentTargetPriorityValue = 0;
            target = null;
        }
        else
        {
            target = targetToAttack.GetComponent<Collider>();
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, 8, LayerMask.GetMask("Attackble"));
        if (colliders.Length > 0)
        {
            Collider highestPriorityTarget = FindHighestPriority(colliders);
            if (highestPriorityTarget != target)
            {
                targetToAttack = highestPriorityTarget.transform;
                currentTargetPriorityValue = highestPriorityTarget.GetComponent<IPriority>().Priority;
                return true;
            }
        }

        return false;
    }


    // just for debugging
    //public void SetIdleMaterial()
    //{
    //    GetComponent<Renderer>().material = idleStateMaterial;
    //}
    //public void SetFollowMaterial()
    //{
    //    GetComponent<Renderer>().material = followStateMaterial;
    //}
    //public void SetAttackMaterial()
    //{
    //    GetComponent<Renderer>().material = attackStateMaterial;
    //}

    // attack/follow range detection gizmos
    private void OnDrawGizmos()
    {
        if (unit == null)
        {
            unit = GetComponent<Unit>();
            if (unit == null) return; // If still null, there's no Unit component attached, so exit
        }
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (unit != null)
        {
            //follow distance
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius /** 0.2f*/); // followdistance unit spherecolliderin(follow/aggro range) radius * unitin skaala

            //attack distance
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, unit.attackRange); // attackingDistance

            //stop attack distance
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, unit.stopAttackDistance);  // stopAttackingDistance
        }
    }
}
