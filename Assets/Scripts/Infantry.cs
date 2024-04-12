using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Infantry : Unit
{
    private void Awake()
    {
        MaxHealth = 150000f;
    }
    
    void Start()
    {
        damage = 10f;
        attackRange = 4f;
        attackSpeed = 2f;
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
    }

    //// Override the TakeDamage method if the infantry has unique behavior when taking damage
    //// esim armor yms 
    //public override void TakeDamage(float amount)
    //{

    //    base.TakeDamage(amount);
    //}
}
