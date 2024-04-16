using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Unit : MonoBehaviour, IDamageable
{
    public float MaxHealth { get; set; } = 75f;
    public float CurrentHealth { get; set; }
    public float damage = 1f;
    public float attackRange = 1f;
    public float attackSpeed = 1f; // attacks per second
    public float movementSpeed = 2f;

    public float attackCooldown;

    protected NavMeshAgent agent;

    void Start()
    {
        movementSpeed = 5f;
        attackCooldown = 1f / attackSpeed;
        CurrentHealth = MaxHealth;
        agent = GetComponent<NavMeshAgent>();
        SetupNavMeshAgent();
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
        if (UnitSelectionManager.Instance.unitsSelected.Contains(gameObject))
        {
            UnitSelectionManager.Instance.unitsSelected.Remove(gameObject);
        }
    }
    protected void SetupNavMeshAgent()
    {
        agent.acceleration = 99999;
        agent.angularSpeed = 200;
        agent.speed = movementSpeed;
        agent.stoppingDistance = 0.5f;
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
    
    public void Heal(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
