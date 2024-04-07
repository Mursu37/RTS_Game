using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour, IDamageable
{
    public float MaxHealth { get; set; } = 100f;
    public float CurrentHealth { get; set; }
    public float damage = 1f;
    public float attackRange = 1f;
    public float attackSpeed = 1f; // attacks per second
    public float movementSpeed = 2f;

    protected float attackCooldown;

    protected NavMeshAgent agent;

    void Start()
    {
        attackCooldown = 1f / attackSpeed;
        CurrentHealth = MaxHealth;
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
    
    public void Damage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
