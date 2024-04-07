using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public float healthPoints = 10f;
    public float damagetest = 1f;
    public float attackRange = 1f;
    public float attackSpeed = 1f; // attacks per second
    public float movementSpeed = 2f;

    protected float attackCooldown;

    protected NavMeshAgent agent;

    void Start()
    {
        attackCooldown = 1f / attackSpeed;

        agent = GetComponent<NavMeshAgent>();
        SetupNavMeshAgent();
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
    }
    protected void SetupNavMeshAgent()
    {
        agent.acceleration = 99999;
        agent.angularSpeed = 200;
        agent.speed = movementSpeed;
        agent.stoppingDistance = 1.5f;
    }


    //public virtual void TakeDamage(float amount)
    //{
    //    healthPoints -= amount;
    //    if (healthPoints <= 0)
    //    {
    //        Destroy(gameObject); 
    //    }
    //}

    
}
