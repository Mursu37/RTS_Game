using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Infantry : Unit
{
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
    
    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
        if (UnitSelectionManager.Instance.unitsSelected.Contains(gameObject))
        { 
            UnitSelectionManager.Instance.unitsSelected.Remove(gameObject);
        }
    }

    //// Override the TakeDamage method if the infantry has unique behavior when taking damage
    //// esim armor yms 
    //public override void TakeDamage(float amount)
    //{

    //    base.TakeDamage(amount);
    //}
}
