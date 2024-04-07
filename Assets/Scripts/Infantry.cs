using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Infantry : Unit
{
     void Start()
    {

        healthPoints = 150f;
        damage = 10f;
        attackRange = 4f;
        attackSpeed = 2f;
        movementSpeed = 3f;
        attackCooldown = 1f / attackSpeed;

      //  SetupNavMeshAgent();

        
    }

    // Override the TakeDamage method if the infantry has unique behavior when taking damage
    // esim armor yms 
    public override void TakeDamage(float amount)
    {
        
        base.TakeDamage(amount);
    }
}
