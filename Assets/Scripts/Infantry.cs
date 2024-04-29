using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Infantry : Unit
{
    public Animator animator;
    public float delay = 2f;
    
    private void Awake()
    {
        MaxHealth = 100f;
    }
    
    void Start()
    {
        damage = 30f;
        attackRange = 3f;
        stopAttackDistance = 3.2f; // t‰m‰n pit‰‰ olla v‰h‰n isompi kuin attack range
        attackSpeed = 0.5f;
        movementSpeed = 3f;
        attackCooldown = 1f / attackSpeed;
        CurrentHealth = MaxHealth;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
        SetupNavMeshAgent();
        


    }

    public override void Die()
    {
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, delay);
        
    }




    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
        if (UnitSelectionManager.Instance.unitsSelected.Contains(gameObject))
        { 
            UnitSelectionManager.Instance.unitsSelected.Remove(gameObject);
        }
    }
    

   
}
