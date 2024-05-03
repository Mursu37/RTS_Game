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
        damage = 10f; // 10
        attackRange = 5f;
        stopAttackDistance = attackRange + 0.2f; // t‰m‰n pit‰‰ olla v‰h‰n isompi kuin attack range
        attackSpeed = 3f; //3
        movementSpeed = 15f; //3
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
