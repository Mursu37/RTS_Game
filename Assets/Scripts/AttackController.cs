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
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius * 0.2f); // followdistance unit spherecolliderin(follow/aggro range) radius * unitin skaala

            //attack distance
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, unit.attackRange); // attackingDistance

            //stop attack distance
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, unit.stopAttackDistance);  // stopAttackingDistance
        }
    }
}
